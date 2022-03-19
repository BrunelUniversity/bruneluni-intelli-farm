using System.Windows;

namespace BrunelUni.IntelliFarm.RenderClient.Windows
{
    public partial class Popup : Window
    {
        private readonly string _message;

        public Popup( string message ) { _message = message; }
        
        public Popup( ) { InitializeComponent( ); }
    }
}