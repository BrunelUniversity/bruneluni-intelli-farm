using System.Windows;
using System.Windows.Controls;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class RenderPage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        public RenderPage( INavigationService wpfNavigationService )
        {
            _wpfNavigationService = wpfNavigationService;
            InitializeComponent( );
        }

        private void RenderButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            // block until all other clients have started
            // render, report into s3 bucket with logged render time
        }
        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}