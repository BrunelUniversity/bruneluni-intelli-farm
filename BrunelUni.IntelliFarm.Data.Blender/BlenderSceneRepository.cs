using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneRepository : ISceneRepository
    {
        // TODO: implement
        public Result Update( RenderDataDto renderOptions ) => Result.Error( "implement" );

        // TODO: implement
        public ObjectResult<RenderDataDto> Read( ) => new ObjectResult<RenderDataDto>
            { Status = OperationResultEnum.Failed, Msg = "implement" };
    }
}