namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IWebClient
    {
        public byte[] GetAsBytes( string endpoint );
    }
}