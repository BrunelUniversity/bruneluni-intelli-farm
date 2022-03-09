using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneRepository : ISceneRepository
    {
        private readonly IRenderManagerService _renderManagerService;
        private readonly ISceneProcessor _sceneProcessor;

        public BlenderSceneRepository( ISceneProcessor sceneProcessor, IRenderManagerService renderManagerService )
        {
            _sceneProcessor = sceneProcessor;
            _renderManagerService = renderManagerService;
        }


        public Result Update( RenderDataDto renderOptions )
        {
            var fileResult = _sceneProcessor.WriteTemp( renderOptions );
            if( fileResult.Status == OperationResultEnum.Failed ) { return fileResult; }

            var processResult = _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "writer",
                false );
            _sceneProcessor.ClearTemp( );
            return processResult.Status == OperationResultEnum.Failed ? processResult : Result.Success( );
        }

        public ObjectResult<RenderDataDto> Read( )
        {
            var processResult = _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "reader",
                false );
            if( processResult.Status == OperationResultEnum.Failed )
            {
                return new ObjectResult<RenderDataDto>
                {
                    Status = OperationResultEnum.Failed,
                    Msg = processResult.Msg
                };
            }

            var readResult = _sceneProcessor.ReadTemp<RenderDataDto>( );
            if( readResult.Status == OperationResultEnum.Failed ) { return readResult; }

            _sceneProcessor.ClearTemp( );
            return new ObjectResult<RenderDataDto>
            {
                Status = OperationResultEnum.Success,
                Value = readResult.Value
            };
        }

        public RayCoverageResultDto GetCoverage( RayCoverageInputDto rayCoverageInputDto )
        {
            return new RayCoverageResultDto
            {
                Percentage = 0.0
            };
        }
    }
}