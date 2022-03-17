﻿using BrunelUni.IntelliFarm.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Microsoft.AspNetCore.Components.Route("device") ]
    public class DeviceController : ControllerBase
    {
        [ HttpPost ]
        public IActionResult CreateDevice( [ FromBody ] ClientDto device )
        {
            return Ok( device );
        }
    }
}