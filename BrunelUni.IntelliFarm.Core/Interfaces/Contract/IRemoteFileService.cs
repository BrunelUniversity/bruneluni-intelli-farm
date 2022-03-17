namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IRemoteFileService
    {
        string Get( string path );
        string Write( string path );
    }
}