using System;
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

        private void RenderNavButton_OnClick( object sender, RoutedEventArgs e )
        {
            _navigationService.NavigateTo( "render-page" );
        }

        private void CreateSceneNavButton_OnClick( object sender, RoutedEventArgs e )
        {
            _navigationService.NavigateTo( "create-scene" );
        }

        private void LargeButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            var myPopup = new Window( );
        }

        private void RenderNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) { throw new NotImplementedException( ); }
        private void CreateProject_OnOnClick( object arg1, RoutedEventArgs arg2 ) { throw new NotImplementedException( ); }
        private void CreateClient_OnOnClick( object arg1, RoutedEventArgs arg2 ) { throw new NotImplementedException( ); }
    }
}