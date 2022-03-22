using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class RenderBlenderManager : IRenderManager
    {
        private readonly RenderMetaDto _renderMetaDto;
        private readonly object _ness;

        public RenderBlenderManager( RenderMetaDto renderMetaDto )
        {
            _renderMetaDto = renderMetaDto;
            _ness = new object( );
        }

        public RenderMetaDto GetRenderInfo( ) => _renderMetaDto;
    }
}