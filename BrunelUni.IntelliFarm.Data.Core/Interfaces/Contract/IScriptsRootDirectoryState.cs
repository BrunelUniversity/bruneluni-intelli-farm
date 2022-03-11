using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    [ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
    public interface IScriptsRootDirectoryState
    {
        public string Directory { get; }
        public string DataScriptsDir { get; }
        public string DataScriptsTempFile { get; }
        public string BlenderDirectory { get; }
        public string BlenderScriptsModulesDirectory { get; }
    }
}