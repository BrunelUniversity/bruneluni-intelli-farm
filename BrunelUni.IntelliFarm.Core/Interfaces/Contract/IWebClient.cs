namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IWebClient
    {
        public string DownloadFile( string endpoint, string filename );
    }
}