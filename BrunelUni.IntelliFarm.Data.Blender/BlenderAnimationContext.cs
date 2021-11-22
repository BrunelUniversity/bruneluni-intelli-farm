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
        private readonly IRenderManagerService _renderManagerService;
        private readonly IRenderManagerFactory _renderManagerFactory;
        private readonly IFileAdapter _fileAdapter;
        private readonly IZipAdapter _zipAdapter;
        private readonly IScriptsRootDirectoryState _scriptsRootDirectoryState;
        private readonly IWebClientAdapter _webClientAdapter;

        public BlenderAnimationContext( IRenderManagerService renderManagerService,
            IRenderManagerFactory renderManagerFactory,
            IFileAdapter fileAdapter,
            IZipAdapter zipAdapter,
            IScriptsRootDirectoryState scriptsRootDirectoryState,
            IWebClientAdapter webClientAdapter )
        {
            _renderManagerService = renderManagerService;
            _renderManagerFactory = renderManagerFactory;
            _fileAdapter = fileAdapter;
            _zipAdapter = zipAdapter;
            _scriptsRootDirectoryState = scriptsRootDirectoryState;
            _webClientAdapter = webClientAdapter;
        }

        public Result Initialize( )
        {
            if( _fileAdapter.Exists( _scriptsRootDirectoryState.BlenderDirectory ).Status == OperationResultEnum.Success
            ) return Result.Success( );
            var webResult = _webClientAdapter.DownloadFile(
                $"{DataApplicationConstants.BlenderBaseUrl}/{DataApplicationConstants.BlenderVersion}.zip", "blender.zip" );
            if( webResult.Status == OperationResultEnum.Failed )
            {
                return webResult;
            }
            var zipResult = _zipAdapter.ExtractToDirectory( "blender.zip", $"{_scriptsRootDirectoryState.Directory}\\blender" );
            return zipResult.Status == OperationResultEnum.Failed ? zipResult : Result.Success( );
        }

        public Result InitializeScene( string filePath )
        {
            if( _fileAdapter.Exists( filePath ).Status == OperationResultEnum.Failed )
            {
                return Result.Error( $"{filePath} was not found" );
            }
            var ext = _fileAdapter.GetFileExtension( filePath ).Value;
            if( ext != ".blend" )
            {
                return Result.Error( $"{filePath} is not of type '.blend'" );
            }
            _renderManagerService.RenderManager = _renderManagerFactory.Factory( new RenderMetaDto
            {
                BlendFilePath = filePath
            } );
            return Result.Success( );
        }
    }
}