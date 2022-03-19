using System.Windows;
using System.Windows.Controls;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class RenderPage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        private readonly ILongRunningTaskDispatcher _longRunningTaskDispatcher;
        private readonly IIntelliFarmFacade _intelliFarmFacade;

        public RenderPage( INavigationService wpfNavigationService,
            ILongRunningTaskDispatcher longRunningTaskDispatcher,
            IIntelliFarmFacade intelliFarmFacade )
        {
            _wpfNavigationService = wpfNavigationService;
            _longRunningTaskDispatcher = longRunningTaskDispatcher;
            _intelliFarmFacade = intelliFarmFacade;
            InitializeComponent( );
        }

        private void RenderButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            var scene = SceneNameTextBox.Text;
            var device = DeviceNameTextBox.Text;
            _longRunningTaskDispatcher.FireAndForget( ( ) =>
            {
                _intelliFarmFacade.Render( scene, device );
            } );
        }


        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}