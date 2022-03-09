using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderRenderEventRepository : IRenderEventRepository
    {
        private readonly IRenderManagerService _renderManagerService;
        private readonly ISceneProcessor _sceneProcessor;

        public BlenderRenderEventRepository( IRenderManagerService renderManagerService,
            ISceneProcessor sceneProcessor )
        {
            _renderManagerService = renderManagerService;
            _sceneProcessor = sceneProcessor;
        }

        public ObjectResult<RenderResultDto> Create( )
        {
            var processorResult = _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "render_frame",
                true );
            if( processorResult.Status == OperationResultEnum.Failed )
            {
                return new ObjectResult<RenderResultDto>
                {
                    Status = processorResult.Status,
                    Msg = processorResult.Msg
                };
            }

            var fileResult = _sceneProcessor.ReadTemp<RenderResultDto>( );
            _sceneProcessor.ClearTemp( );
            return fileResult;
        }
    }
}