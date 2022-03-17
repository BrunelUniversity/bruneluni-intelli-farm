using System.Windows;
using System.Windows.Controls;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class CreateScenePage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        public CreateScenePage( INavigationService wpfNavigationService )
        {
            _wpfNavigationService = wpfNavigationService;
            InitializeComponent( );
        }

        private void UploadSceneButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            // open file dialog, zip file, convert it to bytes
        }

        private void CreateButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            // check that file exists
            // call API
        }
        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}