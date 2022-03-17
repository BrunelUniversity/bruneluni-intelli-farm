using System;
using System.Windows;
using System.Windows.Controls;

namespace BrunelUni.IntelliFarm.RenderClient.Controls
{
    public partial class SmallerButton : UserControl
    {
        public SmallerButton( )
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

        private void HomeButton_OnClick( object sender, RoutedEventArgs e )
        {
            OnClick.Invoke( sender, e );
        }
    }
}