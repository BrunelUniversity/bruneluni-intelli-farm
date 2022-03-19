using System.Windows;
using System.Windows.Controls;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.RenderClient.Windows;

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
                var result = _intelliFarmFacade.Render( scene, device );
                if( result.Status == OperationResultEnum.Failed )
                {
                    new Popup( result.Msg ).ShowDialog( );
                }
            } );
        }


        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}