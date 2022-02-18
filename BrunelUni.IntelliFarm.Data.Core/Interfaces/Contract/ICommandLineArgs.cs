using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    [ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
    public interface ICommandLineArgs
    {
        public string [ ] Args { get; set; }
    }
}