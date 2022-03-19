using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Route("scene") ]
    public class SceneController : ControllerBase
    {
        private readonly IIntelliFarmFacade _intelliFarmFacade;
        private IState _state;

        public SceneController( IIntelliFarmFacade intelliFarmFacade, IState state )
        {
            _intelliFarmFacade = intelliFarmFacade;
            _state = state;
        }
        
        [ HttpPost ]
        public IActionResult CreateScene( [ FromBody ] SceneDto scene )
        {
            _intelliFarmFacade.CreateBucketsFromProject( scene );
            return Ok( scene );
        }
        
        [ HttpGet ]
        public IActionResult GetAllScenes( )
        {
            return Ok( _state.Scenes );
        }
    }
}