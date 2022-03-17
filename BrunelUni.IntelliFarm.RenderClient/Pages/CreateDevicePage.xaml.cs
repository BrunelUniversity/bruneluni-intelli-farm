using System.Windows;
using System.Windows.Controls;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class CreateDevicePage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        private readonly IWebClient _webClient;

        public CreateDevicePage( INavigationService wpfNavigationService, IWebClient webClient )
        {
            _wpfNavigationService = wpfNavigationService;
            _webClient = webClient;
            InitializeComponent( );
        }

        private void CreateButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            var zipBytes = _webClient.GetAsBytes( "calibration-files" );
            // download 80 poly, 100 samples, 100 coverage test file from s3, unzip, render and record time
            // download 0 viewport file from s3, unzip, render and record time
            // call API
        }
        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}