using System.Collections.Generic;
using System.Linq;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.RenderClient.Pages;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public class IntelliFarmFacade : IIntelliFarmFacade
    {
        private readonly IWebClient _webClient;
        private readonly IConfigurationAdapter _configurationAdapter;
        private readonly IZipAdapter _zipAdapter;
        private readonly ISceneCommandFacade _sceneCommandFacade;
        private readonly IAnimationContext _animationContext;
        private readonly IFileAdapter _fileAdapter;

        public IntelliFarmFacade( IWebClient webClient, IConfigurationAdapter configurationAdapter, IZipAdapter zipAdapter, ILoggerAdapter<CreateDevicePage> loggerAdapter, ISceneCommandFacade sceneCommandFacade, IAnimationContext animationContext, IFileAdapter fileAdapter )
        {
            _webClient = webClient;
            _configurationAdapter = configurationAdapter;
            _zipAdapter = zipAdapter;
            _sceneCommandFacade = sceneCommandFacade;
            _animationContext = animationContext;
            _fileAdapter = fileAdapter;
        }

        public void CreateProject( string name, string filePath, string [ ] devices )
        {
            var file = _webClient.DownloadFile( "calibration-files", "calibration.zip" );
            _zipAdapter.ExtractToDirectory( file, _fileAdapter.GetCurrentDirectory( ).Value );
            _animationContext.Initialize(  );
            _animationContext.InitializeScene( $"{_fileAdapter.GetCurrentDirectory( ).Value}\\poly_80_100_coverage.blend" );
            _sceneCommandFacade.SetSceneData( new RenderDataDto
            {
                DiffuseBounces = 0,
                EndFrame = 1,
                StartFrame = 1,
                MaxBounces = 0,
                Samples = 100
            } );
            var times = new List<double>( );
            for( var i = 0; i < 3; i++ )
            {
                times.Add( _sceneCommandFacade.Render( ).RenderTime );
            }

            var avTimeBaseScene = times.Sum( ) / 3;
            _animationContext.InitializeScene( $"{_fileAdapter.GetCurrentDirectory( ).Value}\\vewiport_0.blend" );
            _sceneCommandFacade.SetSceneData( new RenderDataDto
            {
                DiffuseBounces = 0,
                EndFrame = 1,
                StartFrame = 1,
                MaxBounces = 0,
                Samples = 100
            } );
            
            var times0Scene = new List<double>( );
            for( var i = 0; i < 3; i++ )
            {
                times0Scene.Add( _sceneCommandFacade.Render( ).RenderTime );
            }

            var avTime0Scene = times0Scene.Sum( ) / 3;
        }

        public void CreateDevice( string deviceName ) { throw new System.NotImplementedException( ); }

        public void Render( ) { throw new System.NotImplementedException( ); }
    }
}