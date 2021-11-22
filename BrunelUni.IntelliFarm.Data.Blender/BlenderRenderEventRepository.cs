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

        public BlenderRenderEventRepository( IRenderManagerService renderManagerService, ISceneProcessor sceneProcessor )
        {
            _renderManagerService = renderManagerService;
            _sceneProcessor = sceneProcessor;
        }
        
        public ObjectResult<RenderResultDto> Create( )
        {
            return new ObjectResult<RenderResultDto>
            {
                Status = OperationResultEnum.Failed
            };
        }
    }
}