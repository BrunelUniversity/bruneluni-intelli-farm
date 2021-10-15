using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.Data.API.Controllers
{
    [ Route( "render-data" ) ]
    public class RenderDataController : ControllerBase
    {
        private readonly MvcAdapter _mvcAdapter;
        private readonly ISceneRepository _sceneRepository;

        public RenderDataController( MvcAdapter mvcAdapter, ISceneRepository sceneRepository )
        {
            _mvcAdapter = mvcAdapter;
            _sceneRepository = sceneRepository;
        }

        [ HttpGet ]
        public IActionResult GetRenderInfo( )
        {
            _sceneRepository.Read( );
            return _mvcAdapter.Success( "get render info" );
        }

        [ HttpPut ]
        public IActionResult UpdateRenderInfo( [ FromBody ] RenderOptions renderOptions )
        {
            _sceneRepository.Update( renderOptions );
            return _mvcAdapter.OkResult( renderOptions );
        }
    }
}