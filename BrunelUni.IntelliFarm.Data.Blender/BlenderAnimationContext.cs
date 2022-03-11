using System;
using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderAnimationContext : IAnimationContext
    {
        private readonly IFileAdapter _fileAdapter;
        private readonly IPythonBundler _pythonBundler;
        private readonly IRenderManagerFactory _renderManagerFactory;
        private readonly IRenderManagerService _renderManagerService;
        private readonly IScriptsRootDirectoryState _scriptsRootDirectoryState;
        private readonly IWebClientAdapter _webClientAdapter;
        private readonly IZipAdapter _zipAdapter;

        public BlenderAnimationContext( IRenderManagerService renderManagerService,
            IRenderManagerFactory renderManagerFactory,
            IFileAdapter fileAdapter,
            IZipAdapter zipAdapter,
            IScriptsRootDirectoryState scriptsRootDirectoryState,
            IWebClientAdapter webClientAdapter,
            IPythonBundler pythonBundler )
        {
            _renderManagerService = renderManagerService;
            _renderManagerFactory = renderManagerFactory;
            _fileAdapter = fileAdapter;
            _zipAdapter = zipAdapter;
            _scriptsRootDirectoryState = scriptsRootDirectoryState;
            _webClientAdapter = webClientAdapter;
            _pythonBundler = pythonBundler;
        }

        public Result Initialize( )
        {
            if( _fileAdapter.Exists( _scriptsRootDirectoryState.BlenderDirectory ).Status == OperationResultEnum.Success
              )
            {
                _pythonBundler.CopySources( _scriptsRootDirectoryState.BlenderScriptsModulesDirectory,
                    _scriptsRootDirectoryState.DataScriptsDir );
                return Result.Success( );
            }

            var webResult = _webClientAdapter.DownloadFile(
                $"{DataApplicationConstants.BlenderBaseUrl}/{DataApplicationConstants.BlenderVersionFull}.zip",
                "blender.zip" );
            if( webResult.Status == OperationResultEnum.Failed ) { return webResult; }

            var zipResult =
                _zipAdapter.ExtractToDirectory( "blender.zip", $"{_scriptsRootDirectoryState.Directory}\\blender" );
            _pythonBundler.CopySources( _scriptsRootDirectoryState.BlenderScriptsModulesDirectory,
                _scriptsRootDirectoryState.DataScriptsDir );
            return zipResult.Status == OperationResultEnum.Failed ? zipResult : Result.Success( );
        }

        public void InitializeScene( string filePath )
        {
            if( _fileAdapter.Exists( filePath ).Status == OperationResultEnum.Failed )
            {
                throw new ArgumentException( $"{filePath} was not found" );
            }

            var ext = _fileAdapter.GetFileExtension( filePath ).Value;
            if( ext != ".blend" ) { throw new ArgumentException( $"{filePath} is not of type '.blend'" ); }

            _renderManagerService.RenderManager = _renderManagerFactory.Factory( new RenderMetaDto
            {
                BlendFilePath = filePath
            } );
        }
    }
}