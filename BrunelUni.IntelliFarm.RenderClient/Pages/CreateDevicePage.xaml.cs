using System.Windows;
using System.Windows.Controls;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class CreateDevicePage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        private readonly ILongRunningTaskDispatcher _longRunningTaskDispatcher;
        private readonly IIntelliFarmFacade _intelliFarmFacade;
        private readonly IWebClient _client;

        public CreateDevicePage( INavigationService wpfNavigationService,
            ILongRunningTaskDispatcher longRunningTaskDispatcher,
            IIntelliFarmFacade intelliFarmFacade,
            IWebClient client )
        {
            _wpfNavigationService = wpfNavigationService;
            _longRunningTaskDispatcher = longRunningTaskDispatcher;
            _intelliFarmFacade = intelliFarmFacade;
            _client = client;
            InitializeComponent( );
        }

        private void CreateButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            var response = _client.Create( "device", new ClientDto
            {
                Name = "beans",
                TimeFor0PolyViewpoint = 45.5,
                TimeFor80Poly100Coverage0Bounces100Samples = 5.6
            } );
            _longRunningTaskDispatcher
                .FireAndForget( ( ) => _intelliFarmFacade.CreateDevice( NameTextBox.Text ) );
        }

        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}