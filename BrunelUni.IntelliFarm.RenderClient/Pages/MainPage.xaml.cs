using System.Windows;
using System.Windows.Controls;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class MainPage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        public MainPage( INavigationService wpfNavigationService )
        {
            _wpfNavigationService = wpfNavigationService;
            InitializeComponent( );
        }

        private void RenderNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.RenderRouteName );

        private void CreateProject_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.CreateSceneRouteName );

        private void CreateClient_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.CreateDeviceRouteName );
    }
}