using System;
using System.IO;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.API.Controllers
{
    public class FileStorageController : ControllerBase
    {
        private readonly IRemoteFileServiceFactory _remoteFileServiceFactory;
        private readonly IConfigurationAdapter _configurationAdapter;
        private WebAppOptions _options;

        public FileStorageController( IRemoteFileServiceFactory remoteFileServiceFactory,
            IConfigurationAdapter configurationAdapter )
        {
            _remoteFileServiceFactory = remoteFileServiceFactory;
            _configurationAdapter = configurationAdapter;
            _options = _configurationAdapter.Get<WebAppOptions>( );
        }
        
        [ Route("calibration-files") ]
        [ HttpGet ]
        public IActionResult GetCalibrationFiles( )
        {
            var options = _configurationAdapter.Get<WebAppOptions>( );
            var remoteFileService = _remoteFileServiceFactory.Factory( options.AwsId, options.AwsToken );
            var stream = remoteFileService.GetStream( "calibration-files/calibration.zip" );
            return new FileStreamResult( stream, "application/octet-stream" );
        }
        
        [ Route("scene-file") ]
        [ HttpGet ]
        public IActionResult GetSceneFile( [ FromQuery ] string key )
        {
            var options = _configurationAdapter.Get<WebAppOptions>( );
            var remoteFileService = _remoteFileServiceFactory.Factory( options.AwsId, options.AwsToken );
            var stream = remoteFileService.GetStream( key );
            return new FileStreamResult( stream, "application/octet-stream" );
        }
        
        [ Route("blender") ]
        [ HttpGet ]
        public IActionResult GetBlender( )
        {
            var remoteFileService = _remoteFileServiceFactory.Factory( _options.AwsId, _options.AwsToken );
            var stream = remoteFileService.GetStream( "blender/blender.zip" );
            return new FileStreamResult( stream, "application/octet-stream" );
        }

        [ Route( "upload-file" ) ]
        public IActionResult UploadFile( [ FromForm ] IFormFile file )
        {
            using var memoryStream = new MemoryStream( );
            file.OpenReadStream( )
                .CopyTo( memoryStream );

            var key = $"scenes/{Guid.NewGuid( )}.zip";
            _remoteFileServiceFactory
                .Factory( _options.AwsId, _options.AwsToken )
                .CreateFromStream( memoryStream, key );
            return Ok( key );
        }
    }
}