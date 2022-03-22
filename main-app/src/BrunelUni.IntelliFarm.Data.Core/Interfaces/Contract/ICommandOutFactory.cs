using Aidan.Common.Core.Interfaces.Excluded;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ICommandOutFactory : IFactory
    {
        ICommandOut Factory( CommandMetaDto commandMetaDto );
    }
}