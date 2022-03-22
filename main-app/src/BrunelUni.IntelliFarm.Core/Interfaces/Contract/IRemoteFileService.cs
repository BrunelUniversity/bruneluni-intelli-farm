using System.IO;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IRemoteFileService
    {
        public void CreateFromStream( Stream data, string path );
        Stream GetStream( string path );
        string DownloadFile( string path );
    }
}