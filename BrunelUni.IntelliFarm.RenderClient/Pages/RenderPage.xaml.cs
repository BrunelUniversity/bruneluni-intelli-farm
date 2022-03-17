using System.Windows;
using System.Windows.Controls;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class RenderPage : Page
    {
        private readonly NavigationService _navigationService;
        public RenderPage( NavigationService navigationService )
        {
            _navigationService = navigationService;
            InitializeComponent( );
        }

        private void RenderButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            // block until all other clients have started
            // render, report into s3 bucket with logged render time
        }
        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _navigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}