using System.ComponentModel;
using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ISceneCommandFacade
    {
        public Result SetSceneData( RenderDataDto renderOptions );
        public ObjectResult<RenderDataDto> GetSceneData( );
        public RayCoverageResultDto GetSceneAndViewportCoverage( RayCoverageInputDto rayCoverageInputDto );

        [ Description( "triggers a render and returns the time taken to render" ) ]
        public ObjectResult<RenderResultDto> Render( );
    }
}