using System.Windows;
using System.Windows.Controls;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class CreateDevicePage : Page
    {
        private readonly NavigationService _navigationService;
        public CreateDevicePage( NavigationService navigationService )
        {
            _navigationService = navigationService;
            InitializeComponent( );
        }

        private void CreateButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            // download 80 poly, 100 samples, 100 coverage test file from s3, unzip, render and record time
            // download 0 viewport file from s3, unzip, render and record time
            // call API
        }
        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _navigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}