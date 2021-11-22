using Aidan.Common.Core.Enum;
using Aidan.Common.Utils.Web;
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
        public IActionResult CreateRenderEvent( )
        {
            var result = _renderEventRepository.Create( );
            return result.Status == OperationResultEnum.Failed ?
                _mvcAdapter.BadRequestError( result.Msg ) :
                _mvcAdapter.OkResult( new
                {
                    RenderTime = result.Value
                } );
        }
    }
}