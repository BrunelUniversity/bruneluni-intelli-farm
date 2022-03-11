using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Constants;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.Fakes
{
    public class FakeScriptsRootDirectoryState : IScriptsRootDirectoryState
    {
        public string Directory { get; } = TestConstants.Directory;
        public string DataScriptsDir { get; } = TestConstants.DataScriptsDir;
        public string DataScriptsTempFile { get; } = TestConstants.DataScriptsTempFile;
        public string BlenderDirectory { get; } = TestConstants.BlenderDirectory;
        public string BlenderScriptsModulesDirectory { get; } = TestConstants.BlenderScriptsModulesDirectory;
    }
}