using System;
using System.Linq;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Enums;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Description( "used to log actual render time for each bucket" ) ]
    public class BucketController : ControllerBase
    {
        private readonly IState _state;

        public BucketController( IState state ) { _state = state; }
        
        [ HttpPost ]
        [ Route("bucket") ]
        public IActionResult CreateBucket( [ FromBody ] BucketDto bucketDto )
        {
            _state.Buckets.Add( bucketDto );
            return Ok( bucketDto );
        }
        
        [ HttpGet ]
        [ Route("predicted-bucket") ]
        public IActionResult GetForDeviceAndScene(
            [ FromQuery ] string sceneName,
            [ FromQuery ] string device )
        {
            try
            {
                var scene = _state.Scenes.First( x => x.Name == sceneName );
                var client = _state.Clients.First( x => x.Name == device );
                var buckets = _state.Buckets.FindAll( x => x.DeviceId == client.Id &&
                                                           x.SceneId == scene.Id &&
                                                           x.Type == BucketTypeEnum.Predicted );
                return Ok( buckets );
            }
            catch( InvalidOperationException )
            {
                return Ok( new BucketDto [ ] { } );
            }
        }
        
        [ HttpGet ]
        [ Route("actual-bucket") ]
        public IActionResult GetActualBuckets(
            [ FromQuery ] string sceneName,
            [ FromQuery ] string device )
        {
            try
            {
                var scene = _state.Scenes.First( x => x.Name == sceneName );
                var client = _state.Clients.First( x => x.Name == device );
                var buckets = _state.Buckets.FindAll( x => x.DeviceId == client.Id &&
                                                           x.SceneId == scene.Id &&
                                                           x.Type == BucketTypeEnum.Actual );
                return Ok( buckets );
            }
            catch( InvalidOperationException )
            {
                return Ok( new BucketDto [ ] { } );
            }
        }
    }
}