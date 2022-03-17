using BrunelUni.IntelliFarm.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Route("device") ]
    public class DeviceController : ControllerBase
    {
        [ HttpPost ]
        public IActionResult CreateDevice( [ FromBody ] ClientDto device )
        {
            return Ok( device );
        }
    }
}