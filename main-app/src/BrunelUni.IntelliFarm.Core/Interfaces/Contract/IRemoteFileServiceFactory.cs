using Aidan.Common.Core.Interfaces.Excluded;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IRemoteFileServiceFactory : IFactory
    {
        IRemoteFileService Factory( string id, string secret );
    }
}