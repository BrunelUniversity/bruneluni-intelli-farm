using System;
using System.Windows;
using System.Windows.Controls;

namespace BrunelUni.IntelliFarm.RenderClient.Controls
{
    public partial class LargeButton : UserControl
    {
        public LargeButton( )
        {
            InitializeComponent( );
            DataContext = this;
        }

        public event Action<object, RoutedEventArgs> OnClick;
        public string Text { get; set; }

        private void LargeButtonButton_OnClick( object sender, RoutedEventArgs e )
        {
            OnClick.Invoke( sender, e );
        }
    }
}