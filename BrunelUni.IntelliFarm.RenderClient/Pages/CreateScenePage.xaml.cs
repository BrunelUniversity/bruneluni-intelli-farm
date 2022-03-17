using System.Windows;
using System.Windows.Controls;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class CreateScenePage : Page
    {
        private readonly NavigationService _navigationService;
        public CreateScenePage( NavigationService navigationService )
        {
            _navigationService = navigationService;
            InitializeComponent( );
        }

        private void ButtonBase_OnClick( object sender, RoutedEventArgs e )
        {
            _navigationService.NavigateTo( "main" );
        }
    }
}