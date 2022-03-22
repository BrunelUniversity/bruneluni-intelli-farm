using System.ComponentModel;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ISceneCommandFacade
    {
        public void SetSceneData( RenderDataDto renderOptions );
        public RenderDataDto GetSceneData( );
        public RayCoverageResultDto GetSceneAndViewportCoverage( RayCoverageInputDto rayCoverageInputDto );

        public TriangleCountDto GetTriangleCount( );

        [ Description( "triggers a render and returns the time taken to render" ) ]
        public RenderResultDto Render( );
    }
}