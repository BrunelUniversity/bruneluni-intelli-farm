using System.IO;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IRemoteFileService
    {
        string Get( string path );
        Stream GetStream( string path );
        string Write( string path );
    }
}