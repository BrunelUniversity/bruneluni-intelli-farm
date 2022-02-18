using System;
using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Excluded;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    [ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
    public interface IBatchService : IInitialisable
    {
        public Guid Session { get; set; }
    }
}