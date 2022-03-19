using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Route("scene") ]
    public class SceneController : ControllerBase
    {
        private readonly IIntelliFarmFacade _intelliFarmFacade;

        public SceneController( IIntelliFarmFacade intelliFarmFacade ) { _intelliFarmFacade = intelliFarmFacade; }
        
        [ HttpPost ]
        public IActionResult CreateScene( [ FromBody ] SceneDto scene )
        {
            
            return Ok( scene );
        }
    }
}