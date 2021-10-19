using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.Data.API.Controllers
{
    [ Route( "render-data" ) ]
    public class RenderDataController : BaseDataController
    {
        private readonly MvcAdapter _mvcAdapter;
        private readonly ISceneRepository _sceneRepository;
        private readonly IRenderManagerGetter _renderManagerGetter;

        public RenderDataController( MvcAdapter mvcAdapter, ISceneRepository sceneRepository, IRenderManagerGetter renderManagerGetter )
        {
            _mvcAdapter = mvcAdapter;
            _sceneRepository = sceneRepository;
            _renderManagerGetter = renderManagerGetter;
        }

        [ HttpGet ]
        public IActionResult GetRenderInfo( )
        {
            _sceneRepository.Read( );
            return _mvcAdapter.OkResult( _renderManagerGetter.RenderManager.GetRenderInfo(  ) );
        }

        [ HttpPut ]
        public IActionResult UpdateRenderInfo( [ FromBody ] RenderDataDto renderOptions )
        {
            _sceneRepository.Update( renderOptions );
            return _mvcAdapter.OkResult( new RenderDto[] { renderOptions, _renderManagerGetter.RenderManager.GetRenderInfo(  ) } );
        }
    }
}