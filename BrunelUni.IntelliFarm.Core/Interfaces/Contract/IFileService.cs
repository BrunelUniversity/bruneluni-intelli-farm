namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IFileService
    {
        void CreateFileFromBytes( byte [ ] bytes,
            bool local,
            string fileName );
    }
}