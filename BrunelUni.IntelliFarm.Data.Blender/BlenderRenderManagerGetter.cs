using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderRenderManagerGetter : IRenderManagerGetter
    {
        private bool _hasBeenSet;
        private IRenderManager _renderManager;
        public BlenderRenderManagerGetter( ) { _hasBeenSet = false; }
        public IRenderManager RenderManager
        {
            get => _renderManager;
            set
            {
                if( _hasBeenSet ) return;
                _hasBeenSet = true;
                _renderManager = value;
            }
        }
    }
}