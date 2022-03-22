using Aidan.Common.Core.Interfaces.Excluded;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ICommandInFactory : IFactory
    {
        ICommandIn Factory( CommandMetaDto commandMetaDto );
    }
}