using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.Data.API.Controllers
{
    [ Route( "render-data" ) ]
    public class RenderDataController : ControllerBase
    {
        private readonly MvcAdapter _mvcAdapter;

        public RenderDataController( MvcAdapter mvcAdapter ) { _mvcAdapter = mvcAdapter; }

        [ HttpGet ]
        public IActionResult GetRenderInfo( ) => _mvcAdapter.Success( "get render info" );

        [ HttpPut ]
        public IActionResult UpdateRenderInfo( [ FromBody ] RenderOptions renderOptions ) => _mvcAdapter.OkResult( new[]{ renderOptions } );
    }
}