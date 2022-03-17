using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.DataAccess
{
    public class S3RemoteFileService : IRemoteFileService
    {
        public byte [ ] GetBytes( string path ) { throw new System.NotImplementedException( ); }

        public byte [ ] WriteBytes( string path ) { throw new System.NotImplementedException( ); }
    }
}