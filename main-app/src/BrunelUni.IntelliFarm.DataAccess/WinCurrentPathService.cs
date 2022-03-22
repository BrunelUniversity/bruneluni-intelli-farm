using System.IO;
using System.Reflection;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.DataAccess
{
    public class WinCurrentPathService : ICurrentPathService
    {
        private readonly ISolutionDirectoryGetter _solutionDirectoryGetter;

        public WinCurrentPathService( ISolutionDirectoryGetter solutionDirectoryGetter )
        {
            _solutionDirectoryGetter = solutionDirectoryGetter;
        }
        
        public string GetCurrent( bool local )
        {
            return local
                ? _solutionDirectoryGetter.Get( ).Value
                : Path.GetDirectoryName( Assembly.GetExecutingAssembly( ).Location );
        }
    }
}