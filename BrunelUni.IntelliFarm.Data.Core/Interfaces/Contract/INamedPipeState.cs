using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Enums;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    [ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
    [ ThreadSafe ]
    public interface INamedPipeState
    {
        public NamePipeOperationEnum Request { get; set; }
        public string DataIn { get; set; }
        public string DataOut { get; set; }
    }
}