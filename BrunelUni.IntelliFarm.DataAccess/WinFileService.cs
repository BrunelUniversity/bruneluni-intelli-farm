using System.IO;

namespace BrunelUni.IntelliFarm.DataAccess
{
    public class WinFileService
    {
        public void CreateFileFromBytes( byte[] bytes )
        {
            File.WriteAllBytes( "", bytes );
        }
    }
}