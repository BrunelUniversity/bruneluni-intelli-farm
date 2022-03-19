using System.IO;
using System.IO.Compression;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.DataAccess
{
    public class ZipService : IZipService
    {
        public void Zip( string source, string dest )
        {
            using var zip = ZipFile.Open( dest, ZipArchiveMode.Create );
            zip.CreateEntryFromFile( source, Path.GetFileName( source ) );
        }
    }
}