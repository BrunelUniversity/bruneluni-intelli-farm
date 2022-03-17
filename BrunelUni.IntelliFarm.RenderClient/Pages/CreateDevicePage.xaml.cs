using System.Windows;
using System.Windows.Controls;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class CreateDevicePage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        private readonly IWebClient _webClient;
        private readonly IFileService _fileService;
        private readonly IConfigurationAdapter _configurationAdapter;

        public CreateDevicePage( INavigationService wpfNavigationService,
            IWebClient webClient,
            IFileService fileService,
            IConfigurationAdapter configurationAdapter )
        {
            _wpfNavigationService = wpfNavigationService;
            _webClient = webClient;
            _fileService = fileService;
            _configurationAdapter = configurationAdapter;
            InitializeComponent( );
        }

        private void CreateButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            var zipBytes = _webClient.GetAsBytes( "calibration-files" );
            _fileService.CreateFileFromBytes( zipBytes, _configurationAdapter.Get<MainAppOptions>( ).Local,
                "calibration.zip" );
            // download 80 poly, 100 samples, 100 coverage test file from s3, unzip, render and record time
            // download 0 viewport file from s3, unzip, render and record time
            // call API
        }
        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}