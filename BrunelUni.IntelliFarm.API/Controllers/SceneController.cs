using System;
using BrunelUni.IntelliFarm.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Route("scene") ]
    public class SceneController : ControllerBase
    {
        [ HttpPost ]
        public IActionResult CreateScene( [ FromBody ] SceneDto scene )
        {
            return Ok( scene );
        }
        
        [ HttpPost ]
        public IActionResult GetSceneById( [ FromQuery ] string scene )
        {
            var id = Guid.Parse( scene );
            return Ok( new SceneDto
            {
                Id = id
            } );
        }
    }
}