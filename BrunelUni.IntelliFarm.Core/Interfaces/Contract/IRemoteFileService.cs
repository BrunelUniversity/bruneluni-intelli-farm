namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IRemoteFileService
    {
        byte [ ] GetBytes( string path );
        byte [ ] WriteBytes( string path );
    }
}