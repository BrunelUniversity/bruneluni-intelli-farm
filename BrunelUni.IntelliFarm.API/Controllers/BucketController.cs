using System.Linq;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Description( "used to log actual render time for each bucket" ) ]
    [ Route("bucket") ]
    public class BucketController : ControllerBase
    {
        private readonly IState _state;

        public BucketController( IState state ) { _state = state; }
        
        [ HttpPost ]
        public IActionResult CreateBucket( [ FromBody ] BucketDto bucketDto )
        {
            _state.Buckets.Add( bucketDto );
            return Ok( bucketDto );
        }
        
        [ HttpGet ]
        public IActionResult GetForDeviceAndScene(
            [ FromQuery ] string sceneName,
            [ FromQuery ] string device )
        {
            var scene = _state.Scenes.First( x => x.Name == sceneName );
            var client = _state.Clients.First( x => x.Name == device );
            var buckets = _state.Buckets.FindAll( x => x.DeviceId == client.Id && x.SceneId == scene.Id );
            return Ok( buckets );
        }
    }
}