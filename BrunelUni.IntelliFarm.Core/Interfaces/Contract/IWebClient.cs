using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IWebClient
    {
        public string DownloadFile( string endpoint, string filename );
        public string UploadFile( string endpoint, string filename );
        public WebDto Create<T>( string endpoint, T body );
        public WebDto Get( string endpoint );
    }
}