using Aidan.Common.Core.Enum;
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

        public SceneCommandController( MvcAdapter mvcAdapter, ISceneCommandFacade sceneCommandFacade,
            IRenderManagerService renderManagerService )
        {
            _mvcAdapter = mvcAdapter;
            _sceneCommandFacade = sceneCommandFacade;
        }

        [ HttpGet ]
        [ Route( "render-data" ) ]
        public IActionResult GetRenderInfo( )
        {
            var result = _sceneCommandFacade.GetSceneData( );
            return result.Status == OperationResultEnum.Success
                ? _mvcAdapter.OkResult( result.Value )
                : _mvcAdapter.BadRequestError( result.Msg );
        }

        [ HttpPut ]
        [ Route( "render-data" ) ]
        public IActionResult UpdateRenderInfo( [ FromBody ] RenderDataDto renderOptions )
        {
            var result = _sceneCommandFacade.SetSceneData( renderOptions );
            return result.Status == OperationResultEnum.Success
                ? _mvcAdapter.Success( "file ammended" )
                : _mvcAdapter.BadRequestError( result.Msg );
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
        public IActionResult CreateRenderEvent( )
        {
            var result = _sceneCommandFacade.Render( );
            return result.Status == OperationResultEnum.Failed
                ? _mvcAdapter.BadRequestError( result.Msg )
                : _mvcAdapter.OkResult( result.Value );
        }
    }
}