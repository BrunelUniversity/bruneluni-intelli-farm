using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Route("device") ]
    public class DeviceController : ControllerBase
    {
        private readonly IState _state;

        public DeviceController( IState state ) { _state = state; }
        
        [ HttpPost ]
        public IActionResult CreateDevice( [ FromBody ] ClientDto device )
        {
            _state.Clients.Add( device );
            return Ok( device );
        }
    }
}