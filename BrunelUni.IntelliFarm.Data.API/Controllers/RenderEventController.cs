using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.Data.API.Controllers
{
    [ Route( "render-event" ) ]
    public class RenderEventController : ControllerBase
    {
        private readonly IRenderRepository _renderRepository;
        private readonly MvcAdapter _mvcAdapter;

        public RenderEventController( IRenderRepository renderRepository, MvcAdapter mvcAdapter )
        {
            _renderRepository = renderRepository;
            _mvcAdapter = mvcAdapter;
        }

        [ HttpPost ]
        public IActionResult CreateRenderEvent( [ FromBody ] RenderOptions renderOptions )
        {
            _renderRepository.Create( renderOptions );
            return _mvcAdapter.OkResult( renderOptions );
        }
    }
}