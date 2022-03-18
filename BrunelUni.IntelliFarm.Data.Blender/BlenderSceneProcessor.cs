using System.Diagnostics;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneProcessor : ISceneProcessor
    {
        private readonly ILoggerAdapter<ISceneProcessor> _loggerAdapter;

        public BlenderSceneProcessor( ILoggerAdapter<ISceneProcessor> loggerAdapter )
        {
            _loggerAdapter = loggerAdapter;
        }
        
        public void RunSceneProcess( string command, string args )
        {
            _loggerAdapter.LogInfo( $"running command: {command} with args: {args}" );
            Process.Start( command, args );
        }
    }
}