using System.Windows.Controls;

namespace BrunelUni.IntelliFarm.RenderClient.Controls
{
    public partial class TitleBlock : UserControl
    {
        public TitleBlock( )
        {
            InitializeComponent( );
            DataContext = this;
        }

        public string Text { get; set; }
    }
}