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
        private void CreateButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) { throw new System.NotImplementedException( ); }
        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _navigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}