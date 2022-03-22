using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    [ Service( Scope = ServiceLifetimeEnum.Scoped ) ]
    public interface IRenderManager
    {
        RenderMetaDto GetRenderInfo( );
    }
}