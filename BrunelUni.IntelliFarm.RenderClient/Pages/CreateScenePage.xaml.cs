using System.Windows;
using System.Windows.Controls;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Enums;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient.Pages
{
    public partial class CreateScenePage : Page
    {
        private readonly INavigationService _wpfNavigationService;
        private readonly IWebClient _webClient;

        public CreateScenePage( INavigationService wpfNavigationService,
            IWebClient webClient )
        {
            _wpfNavigationService = wpfNavigationService;
            _webClient = webClient;
            InitializeComponent( );
        }

        private void UploadSceneButton_OnOnClick( object arg1, RoutedEventArgs arg2 )
        {
            _webClient.Get( "bucket", "sceneName=some&device=somedevice" );
            _webClient.Create( "bucket", new BucketDto
            {
                Type = BucketTypeEnum.Actual
            } );
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