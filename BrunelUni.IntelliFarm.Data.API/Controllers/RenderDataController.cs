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
        private readonly IRenderManagerService _renderManagerService;

        public RenderDataController( MvcAdapter mvcAdapter, ISceneRepository sceneRepository, IRenderManagerService renderManagerService )
        {
            _mvcAdapter = mvcAdapter;
            _sceneRepository = sceneRepository;
            _renderManagerService = renderManagerService;
        }

        [ HttpGet ]
        public IActionResult GetRenderInfo( )
        {
            _sceneRepository.Read( );
            return _mvcAdapter.OkResult( _renderManagerService.RenderManager.GetRenderInfo(  ) );
        }

        [ HttpPut ]
        public IActionResult UpdateRenderInfo( [ FromBody ] RenderDataDto renderOptions )
        {
            _sceneRepository.Update( renderOptions );
            return _mvcAdapter.OkResult( new RenderDto[] { renderOptions, _renderManagerService.RenderManager.GetRenderInfo(  ) } );
        }
    }
}