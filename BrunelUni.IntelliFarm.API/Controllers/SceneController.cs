using BrunelUni.IntelliFarm.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Route("scene") ]
    public class SceneController : ControllerBase
    {
        public SceneController( )
        {
            
        }
        
        [ HttpPost ]
        public IActionResult CreateScene( [ FromBody ] SceneDto scene )
        {
            return Ok( scene );
        }
    }
}