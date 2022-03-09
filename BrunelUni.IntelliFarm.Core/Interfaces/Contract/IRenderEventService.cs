using System;
using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract{

    [ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
    public interface IRenderEventService
    {
        event Action<(SceneTinyType, RenderTimeTinyType)> FrameRendered;
    }
}