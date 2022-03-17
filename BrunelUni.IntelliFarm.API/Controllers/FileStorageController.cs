using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    public class FileStorageController : ControllerBase
    {
        [ Route("calibration-files") ]
        [ HttpGet ]
        public IActionResult GetCalibrationFiles( )
        {
            return Ok( "project created successfully" );
        }
    }
}