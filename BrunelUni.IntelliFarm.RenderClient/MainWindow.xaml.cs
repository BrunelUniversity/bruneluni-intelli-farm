using System;
using System.Windows;
using BrunelUni.IntelliFarm.RenderClient.Pages;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public partial class MainWindow : Window
    {
        private NavigationService _navigationService;

        public MainWindow( )
        {
            _navigationService = new NavigationService( );
            _navigationService.Navigate += OnNavigate;
            InitializeComponent( );
            _mainFrame.Navigate( new MainPage( _navigationService ) );
        }

        private void OnNavigate( string obj )
        {
            switch( obj )
            {
                case "main":
                    _mainFrame.Navigate( new MainPage( _navigationService ) );
                    break;
                case "create-project":
                    _mainFrame.Navigate( new CreateScenePage( _navigationService ) );
                    break;
                default:
                    throw new ArgumentException( $"{obj} route does not exist" );
            }
        }
    }
}