using System.Collections.Generic;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Description( "used to log actual render time for each bucket" ) ]
    [ Route("bucket") ]
    public class BucketController : ControllerBase
    {
        [ HttpPost ]
        public IActionResult CreateBucket( [ FromBody ] BucketDto bucketDto )
        {
            return Ok( bucketDto );
        }
        
        [ HttpGet ]
        public IActionResult GetForDeviceAndScene(
            [ FromQuery ] string sceneName,
            [ FromQuery ] string device )
        {
            return Ok( new List<BucketDto>
            {
                new BucketDto(  ),
                new BucketDto
                {
                    Type = BucketTypeEnum.Predicted
                }
            } );
        }
    }
}