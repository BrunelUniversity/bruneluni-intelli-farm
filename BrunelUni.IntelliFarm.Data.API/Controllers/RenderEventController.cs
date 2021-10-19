using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.Data.API.Controllers
{
    [ Route( "render-event" ) ]
    public class RenderEventController : BaseDataController
    {
        private readonly IRenderEventRepository _renderEventRepository;
        private readonly MvcAdapter _mvcAdapter;

        public RenderEventController( IRenderEventRepository renderEventRepository, MvcAdapter mvcAdapter )
        {
            _renderEventRepository = renderEventRepository;
            _mvcAdapter = mvcAdapter;
        }

        [ HttpPost ]
        public IActionResult CreateRenderEvent( [ FromBody ] RenderEventDto renderOptions )
        {
            _renderEventRepository.Create( renderOptions );
            return _mvcAdapter.OkResult( renderOptions );
        }
    }
}