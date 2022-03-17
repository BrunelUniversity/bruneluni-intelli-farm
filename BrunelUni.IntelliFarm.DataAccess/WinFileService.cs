using System.IO;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.DataAccess
{
    public class WinFileService
    {
        private readonly ICurrentPathService _currentPathService;

        public WinFileService( ICurrentPathService currentPathService )
        {
            _currentPathService = currentPathService;
        }
        
        public void CreateFileFromBytes( byte[] bytes,
            bool local,
            string fileName )
        {
            File.WriteAllBytes( $"{_currentPathService.GetCurrent( local )}\\{fileName}", bytes );
        }
    }
}