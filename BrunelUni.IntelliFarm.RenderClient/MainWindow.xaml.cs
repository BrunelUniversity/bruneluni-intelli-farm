using System;
using System.Windows;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.RenderClient.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public partial class MainWindow : Window
    {
        private readonly IHost _host;
        private INavigationService WpfNavigationService => _host.Services.GetService<INavigationService>( )!;

        public MainWindow( )
        {
            _host = Host.CreateDefaultBuilder( )
                .ConfigureServices( ( hostContext, services ) =>
                    services.BindCrosscuttingLayer( )
                        .AddTransient<CreateDevicePage>( )
                        .AddTransient<MainPage>( )
                        .AddTransient<RenderPage>( )
                        .AddTransient<CreateScenePage>( ) )
                .Build( );
            
            InitializeComponent( );
            WpfNavigationService.Navigate += OnNavigate;
            WpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
        }

        private void OnNavigate( string obj )
        {
            var pageType = GetPageRoute( obj );
            _mainFrame.Navigate( _host.Services.GetService( pageType ) );
        }

        private Type GetPageRoute( string route ) =>
            route switch
            {
                AppConstants.MainPageRouteName => typeof( MainPage ),
                AppConstants.CreateSceneRouteName => typeof( CreateScenePage ),
                AppConstants.CreateDeviceRouteName => typeof( CreateDevicePage ),
                AppConstants.RenderRouteName => typeof( RenderPage ),
                _ => throw new ArgumentException( "route not found" )
            };
    }
}