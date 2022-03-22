using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.API.Filters;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.Data.API.Controllers
{
    [ ServiceFilter( typeof( RenderFileFilter ), Order = 1 ) ]
    [ ServiceFilter( typeof( BlenderInstallFilter ), Order = 2 ) ]
    public class SceneCommandController
    {
        private readonly MvcAdapter _mvcAdapter;
        private readonly ISceneCommandFacade _sceneCommandFacade;

        public SceneCommandController( MvcAdapter mvcAdapter, ISceneCommandFacade sceneCommandFacade )
        {
            _mvcAdapter = mvcAdapter;
            _sceneCommandFacade = sceneCommandFacade;
        }

        [ HttpGet ]
        [ Route( "render-data" ) ]
        public IActionResult GetRenderData( )
        {
            var result = _sceneCommandFacade.GetSceneData( );
            return _mvcAdapter.OkResult( result );
        }

        [ HttpPut ]
        [ Route( "render-data" ) ]
        public IActionResult SetRenderData( [ FromBody ] RenderDataDto data )
        {
            _sceneCommandFacade.SetSceneData( data );
            return _mvcAdapter.Success( "file ammended" );
        }

        [ HttpGet ]
        [ Route( "coverage" ) ]
        public IActionResult GetCoverage( [ FromBody ] RayCoverageInputDto rayCoverage )
        {
            var result = _sceneCommandFacade.GetSceneAndViewportCoverage( rayCoverage );
            return _mvcAdapter.OkResult( result );
        }

        [ HttpPost ]
        [ Route( "render" ) ]
        public IActionResult Render( )
        {
            var result = _sceneCommandFacade.Render( );
            return _mvcAdapter.OkResult( result );
        }

        [ HttpGet ]
        [ Route( "triangle-count" ) ]
        public IActionResult GetTriangles( )
        {
            var result = _sceneCommandFacade.GetTriangleCount( );
            return _mvcAdapter.OkResult( result );
        }
    }
}