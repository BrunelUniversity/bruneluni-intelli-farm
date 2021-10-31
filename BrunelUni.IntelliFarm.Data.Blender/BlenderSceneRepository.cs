using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneRepository : ISceneRepository
    {
        private readonly ISceneProcessor _sceneProcessor;
        private readonly IRenderManagerService _renderManagerService;

        public BlenderSceneRepository( ISceneProcessor sceneProcessor, IRenderManagerService renderManagerService )
        {
            _sceneProcessor = sceneProcessor;
            _renderManagerService = renderManagerService;
        }


        public Result Update( RenderDataDto renderOptions )
        {
            _sceneProcessor.WriteTemp( renderOptions );
            _sceneProcessor.RunSceneProcessAndExit( _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "writer",
                false );
            return Result.Success( );
        }

        // TODO: implement
        public ObjectResult<RenderDataDto> Read( ) => new ObjectResult<RenderDataDto>
            { Status = OperationResultEnum.Failed, Msg = "implement" };
    }
}