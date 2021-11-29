using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderRenderManagerService : IRenderManagerService
    {
        public IRenderManager RenderManager { get; set; }
    }
}