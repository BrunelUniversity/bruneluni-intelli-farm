using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class ScriptsRootDirectoryState : IScriptsRootDirectoryState
    {
        public ScriptsRootDirectoryState( IConfigurationAdapter configurationAdapter,
            ICurrentPathService currentPathService )
        {
            ScriptsRootDirectoryDto = new ScriptsRootDirectoryDto
            {
                Directory = currentPathService.GetCurrent( configurationAdapter.Get<MainAppOptions>( ).Local )
            };
            ScriptsRootDirectoryDto.DataScriptsDir = $"{ScriptsRootDirectoryDto.Directory}\\blender_api";
            ScriptsRootDirectoryDto.DataScriptsTempFile = $"{ScriptsRootDirectoryDto.Directory}\\temp\\render.json";
            var blendDir =
                $"{ScriptsRootDirectoryDto.Directory}\\blender\\{DataApplicationConstants.BlenderVersionFull}";
            ScriptsRootDirectoryDto.BlenderDirectory = $"{blendDir}\\blender.exe";
            ScriptsRootDirectoryDto.BlenderScriptsModulesDirectory =
                $"{blendDir}\\{DataApplicationConstants.BlenderVersionShort}\\scripts\\modules";
        }

        public ScriptsRootDirectoryDto ScriptsRootDirectoryDto { get; }
    }
}