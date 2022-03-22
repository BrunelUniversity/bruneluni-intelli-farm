using System;
using System.Text.Json;
using System.Windows;
using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Utils;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.Data.DIModule;
using BrunelUni.IntelliFarm.DIModule;
using BrunelUni.IntelliFarm.RenderClient.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
                        .BindIntelliFarm( )
                        .BindDataLayer( )
                        .AddTransient<CreateDevicePage>( )
                        .AddTransient<MainPage>( )
                        .AddTransient<RenderPage>( )
                        .AddTransient<CreateScenePage>( )
                        .RemoveAll( typeof( ILoggerAdapter<> ) )
                        .AddTransient( typeof( ILoggerAdapter<> ), typeof( WpfLogger<> ) )
                        .AddTransient<JsonNamingPolicy, SnakeCaseJsonNamingPolicy>( )
                        .AddTransient<ISerializer, JsonSnakeCaseSerialzier>( ) )
                .Build( );
            
            InitializeComponent( );
            WpfNavigationService.Navigate += OnNavigate;
            WpfNavigationService.NavigateTo( AppConstants.MainPageRouteName );
            ClearLogs( );
        }

        private void ClearLogs( )
        {
            var fileService = _host.Services.GetService<IFileAdapter>( );
            var logsFile = $"{fileService?.GetCurrentDirectory( ).Value}\\current-logs.txt";
            var result = fileService?.WriteFile( logsFile, "" );
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