using System;
using System.IO;
using System.Net;
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
        private readonly IConfigurationAdapter _configurationAdapter;
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
            IPythonBundler pythonBundler,
            IConfigurationAdapter configurationAdapter )
        {
            _renderManagerService = renderManagerService;
            _renderManagerFactory = renderManagerFactory;
            _fileAdapter = fileAdapter;
            _zipAdapter = zipAdapter;
            _scriptsRootDirectoryState = scriptsRootDirectoryState;
            _webClientAdapter = webClientAdapter;
            _pythonBundler = pythonBundler;
            _configurationAdapter = configurationAdapter;
        }

        public void Initialize( )
        {
            if( _fileAdapter.Exists( _scriptsRootDirectoryState.ScriptsRootDirectoryDto.BlenderDirectory ).Status ==
                OperationResultEnum.Success )
            {
                _pythonBundler.Bundle(
                    _scriptsRootDirectoryState.ScriptsRootDirectoryDto.BlenderScriptsModulesDirectory,
                    _scriptsRootDirectoryState.ScriptsRootDirectoryDto.DataScriptsDir );
                return;
            }

            var webResult = _webClientAdapter.DownloadFile(
                $"{DataApplicationConstants.BlenderBaseUrl}/{DataApplicationConstants.BlenderVersionFull}.zip",
                "blender.zip" );
            if( webResult.Status == OperationResultEnum.Failed )
                throw new WebException( $"failing to download file msg: {webResult.Msg}" );

            var zipResult =
                _zipAdapter.ExtractToDirectory( "blender.zip",
                    $"{_scriptsRootDirectoryState.ScriptsRootDirectoryDto.Directory}\\blender" );
            if( zipResult.Status == OperationResultEnum.Failed )
                throw new IOException( $"failed to zip file {zipResult.Msg}" );
            _pythonBundler.Bundle( _scriptsRootDirectoryState.ScriptsRootDirectoryDto.BlenderScriptsModulesDirectory,
                _scriptsRootDirectoryState.ScriptsRootDirectoryDto.DataScriptsDir );
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