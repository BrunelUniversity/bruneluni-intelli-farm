using System.Windows;
using System.Windows.Threading;

namespace BrunelUni.IntelliFarm.RenderClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App( ) => DispatcherUnhandledException += OnDispatcherUnhandledException;

        private void OnDispatcherUnhandledException( object sender, DispatcherUnhandledExceptionEventArgs e ) =>
            LoggerExtensions.GetLogger<App>().LogError( $"exception: {e.Exception.GetType( ).Name} message: {e.Exception.Message}" );
    }
}