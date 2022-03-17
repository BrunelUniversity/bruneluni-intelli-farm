using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    public class FileStorageController : ControllerBase
    {
        private readonly IRemoteFileServiceFactory _remoteFileServiceFactory;
        private readonly IConfigurationAdapter _configurationAdapter;

        public FileStorageController( IRemoteFileServiceFactory remoteFileServiceFactory,
            IConfigurationAdapter configurationAdapter )
        {
            _remoteFileServiceFactory = remoteFileServiceFactory;
            _configurationAdapter = configurationAdapter;
        }
        
        [ Route("calibration-files") ]
        [ HttpGet ]
        public IActionResult GetCalibrationFiles( )
        {
            var options = _configurationAdapter.Get<AppOptions>( );
            var remoteFileService = _remoteFileServiceFactory.Factory( options.AwsId, options.AwsToken );
            var stream = remoteFileService.GetStream( "calibration-files/calibration.zip" );
            return new FileStreamResult( stream, "application/octet-stream" );
        }
        
        [ Route("blender") ]
        [ HttpGet ]
        public IActionResult GetBlender( )
        {
            var options = _configurationAdapter.Get<AppOptions>( );
            var remoteFileService = _remoteFileServiceFactory.Factory( options.AwsId, options.AwsToken );
            var stream = remoteFileService.GetStream( "blender/blender.zip" );
            return new FileStreamResult( stream, "application/octet-stream" );
        }
    }
}