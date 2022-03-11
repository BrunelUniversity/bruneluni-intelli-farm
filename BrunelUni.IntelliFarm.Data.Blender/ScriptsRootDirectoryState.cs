using System.IO;
using System.Reflection;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class ScriptsRootDirectoryState : IScriptsRootDirectoryState
    {
        public ScriptsRootDirectoryState( IConfigurationAdapter configurationAdapter,
            ISolutionDirectoryGetter solutionDirectoryGetter )
        {
            Directory = configurationAdapter.Get<AppOptions>( ).Local
                ? solutionDirectoryGetter.Get( ).Value
                : Path.GetDirectoryName( Assembly.GetExecutingAssembly( ).Location );
            DataScriptsDir = $"{Directory}\\blender_api";
            DataScriptsTempFile = $"{Directory}\\temp\\render.json";
            var blendDir = $"{Directory}\\blender\\{DataApplicationConstants.BlenderVersionFull}";
            BlenderDirectory = $"{blendDir}\\blender.exe";
            BlenderScriptsModulesDirectory =
                $"{blendDir}\\{DataApplicationConstants.BlenderVersionShort}\\scripts\\modules";
        }

        public string Directory { get; }
        public string DataScriptsDir { get; }
        public string DataScriptsTempFile { get; }
        public string BlenderDirectory { get; }
        public string BlenderScriptsModulesDirectory { get; }
    }
}