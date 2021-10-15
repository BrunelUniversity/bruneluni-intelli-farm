using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneRepository : ISceneRepository
    {
        // TODO: implement
        public Result Update( RenderOptions renderOptions ) => Result.Error( "implement" );

        // TODO: implement
        public ObjectResult<RenderOptions> Read( ) => new ObjectResult<RenderOptions>
            { Status = OperationResultEnum.Failed, Msg = "implement" };
    }
}