namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IWebClient
    {
        public string DownloadFile( string endpoint, string filename );
        public T Create<T>( string endpoint, T body );
    }
}