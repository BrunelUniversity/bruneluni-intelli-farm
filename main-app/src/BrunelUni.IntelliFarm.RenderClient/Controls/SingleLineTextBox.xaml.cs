using System.Windows.Controls;

namespace BrunelUni.IntelliFarm.RenderClient.Controls
{
    public partial class SingleLineTextBox : UserControl
    {
        public SingleLineTextBox( )
        {
            InitializeComponent( );
            DataContext = this;
        }
        public string Text { get; set; }
    }
}