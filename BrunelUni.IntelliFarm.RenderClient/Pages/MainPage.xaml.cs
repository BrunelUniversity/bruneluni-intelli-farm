using System.Windows;
using System.Windows.Controls;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class MainPage : Page
    {
        private readonly NavigationService _navigationService;
        public MainPage( NavigationService navigationService )
        {
            _navigationService = navigationService;
            InitializeComponent( );
        }

        private void RenderNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _navigationService.NavigateTo( AppConstants.RenderRouteName );

        private void CreateProject_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _navigationService.NavigateTo( AppConstants.CreateSceneRouteName );

        private void CreateClient_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _navigationService.NavigateTo( AppConstants.CreateDeviceRouteName );
    }
}