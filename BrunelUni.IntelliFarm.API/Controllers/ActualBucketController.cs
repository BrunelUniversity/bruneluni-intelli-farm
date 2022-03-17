using BrunelUni.IntelliFarm.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    [ Description( "used to log actual render time for each bucket" ) ]
    [ Route("actual-bucket") ]
    public class ActualBucketController : ControllerBase
    {
        [ HttpPost ]
        public IActionResult Create( [ FromBody ] BucketDto bucketDto )
        {
            return Ok( bucketDto );
        }
    }
}