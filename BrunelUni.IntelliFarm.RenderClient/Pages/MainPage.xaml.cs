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

        private void ButtonBase_OnClick( object sender, RoutedEventArgs e )
        {
            _navigationService.NavigateTo( "create-project" );
        }
    }
}