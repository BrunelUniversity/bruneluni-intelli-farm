using System;
using System.Windows;
using System.Windows.Controls;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using Microsoft.Win32;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class CreateScenePage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        private readonly ILongRunningTaskDispatcher _longRunningTaskDispatcher;
        private readonly IIntelliFarmFacade _intelliFarmFacade;
        private string _path = "";
        
        public CreateScenePage( INavigationService wpfNavigationService,
            ILongRunningTaskDispatcher longRunningTaskDispatcher,
            IIntelliFarmFacade intelliFarmFacade )
        {
            _wpfNavigationService = wpfNavigationService;
            _longRunningTaskDispatcher = longRunningTaskDispatcher;
            _intelliFarmFacade = intelliFarmFacade;
            InitializeComponent( );
        }

        private void UploadSceneButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            var fileDialog = new OpenFileDialog( );
            var result = fileDialog.ShowDialog( );
            _path = fileDialog.FileName;
        }

        private void CreateButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            var scene = SceneNameTextBox.Text;
            var devices = DeviceNamesTextBox.Text.Split( ',' );
            _longRunningTaskDispatcher
                .FireAndForget( ( ) =>
                {
                    var result = _intelliFarmFacade
                        .CreateProject( scene, _path, devices );
                    if( result.Status == OperationResultEnum.Failed )
                    {
                        throw new Exception( result.Msg );
                    }
                } );
        }

        private void HomeNavButton_OnOnClick( object arg1, RoutedEventArgs arg2 ) =>
            _wpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
    }
}