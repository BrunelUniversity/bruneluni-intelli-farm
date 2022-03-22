using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Constants;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.Fakes
{
    public class FakeScriptsRootDirectoryState : IScriptsRootDirectoryState
    {
        public ScriptsRootDirectoryDto ScriptsRootDirectoryDto { get; } = new ScriptsRootDirectoryDto
        {
            Directory = TestConstants.Directory,
            DataScriptsDir = TestConstants.DataScriptsDir,
            DataScriptsTempFile = TestConstants.DataScriptsTempFile,
            BlenderDirectory = TestConstants.BlenderDirectory,
            BlenderScriptsModulesDirectory = TestConstants.BlenderScriptsModulesDirectory
        };
    }
}