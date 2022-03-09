using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ISceneRepository
    {
        public Result Update( RenderDataDto renderOptions );
        public ObjectResult<RenderDataDto> Read( );
        public RayCoverageResultDto GetCoverage( RayCoverageInputDto rayCoverageInputDto );
    }
}