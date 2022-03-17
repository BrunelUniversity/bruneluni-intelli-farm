using System;
using System.Windows;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.RenderClient.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public partial class MainWindow : Window
    {
        private readonly IHost _host;
        private NavigationService NavigationService => _host.Services.GetService<NavigationService>( )!;

        public MainWindow( )
        {
            _host = Host.CreateDefaultBuilder( )
                .ConfigureServices( ( hostContext, services ) =>
                    services.BindCrosscuttingLayer( )
                        .AddSingleton<NavigationService>( )
                        .AddTransient<CreateDevicePage>( )
                        .AddTransient<MainPage>( )
                        .AddTransient<RenderPage>( )
                        .AddTransient<CreateScenePage>( ) )
                .Build( );
            
            InitializeComponent( );
            NavigationService.Navigate += OnNavigate;
            NavigationService.NavigateTo( AppConstants.MainPageRouteName );
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