using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderAnimationContext : IAnimationContext
    {
        private readonly IRenderManagerService _renderManagerService;
        private readonly IRenderManagerFactory _renderManagerFactory;
        private readonly IFileAdapter _fileAdapter;

        public BlenderAnimationContext( IRenderManagerService renderManagerService,
            IRenderManagerFactory renderManagerFactory,
            IFileAdapter fileAdapter )
        {
            _renderManagerService = renderManagerService;
            _renderManagerFactory = renderManagerFactory;
            _fileAdapter = fileAdapter;
        }

        public Result Initialize( string filePath )
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