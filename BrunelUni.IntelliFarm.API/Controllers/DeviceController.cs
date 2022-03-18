using BrunelUni.IntelliFarm.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Route("device") ]
    public class DeviceController : ControllerBase
    {
        private readonly State _state;

        public DeviceController( State state ) { _state = state; }
        
        [ HttpPost ]
        public IActionResult CreateDevice( [ FromBody ] ClientDto device )
        {
            _state.Clients.Add( device );
            return Ok( device );
        }
    }
}