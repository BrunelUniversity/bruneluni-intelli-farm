using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ISceneRepository
    {
        public Result Update( RenderOptions renderOptions );
        public ObjectResult<RenderOptions> Read( );
    }
}