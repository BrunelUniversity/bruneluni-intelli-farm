using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class CreateDevicePage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        private readonly IWebClient _webClient;
        private readonly IConfigurationAdapter _configurationAdapter;
        private readonly IZipAdapter _zipAdapter;
        private readonly ILoggerAdapter<CreateDevicePage> _loggerAdapter;
        private readonly ISceneCommandFacade _sceneCommandFacade;
        private readonly IAnimationContext _animationContext;
        private readonly IFileAdapter _fileAdapter;

        public CreateDevicePage( INavigationService wpfNavigationService,
            IWebClient webClient,
            IConfigurationAdapter configurationAdapter,
            IZipAdapter zipAdapter,
            ILoggerAdapter<CreateDevicePage> loggerAdapter,
            ISceneCommandFacade sceneCommandFacade,
            IAnimationContext animationContext,
            IFileAdapter fileAdapter )
        {
            _wpfNavigationService = wpfNavigationService;
            _webClient = webClient;
            _configurationAdapter = configurationAdapter;
            _zipAdapter = zipAdapter;
            _loggerAdapter = loggerAdapter;
            _sceneCommandFacade = sceneCommandFacade;
            _animationContext = animationContext;
            _fileAdapter = fileAdapter;
            InitializeComponent( );
        }

        private async void CreateButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            _loggerAdapter.LogInfo( "test" );
            Task.Run( ( ) =>
            {
                var file = _webClient.DownloadFile( "calibration-files", "calibration.zip" );
                _zipAdapter.ExtractToDirectory( file, Directory.GetCurrentDirectory( ) );
                _animationContext.Initialize(  );
                _animationContext.InitializeScene( $"{Directory.GetCurrentDirectory( )}\\poly_80_100_coverage.blend" );
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
                _animationContext.InitializeScene( $"{Directory.GetCurrentDirectory( )}\\vewiport_0.blend" );
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
            } );
        }
        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}