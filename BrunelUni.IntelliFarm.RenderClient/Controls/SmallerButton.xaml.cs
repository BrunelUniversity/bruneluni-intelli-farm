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
        public string Text { get; set; }
    }
}