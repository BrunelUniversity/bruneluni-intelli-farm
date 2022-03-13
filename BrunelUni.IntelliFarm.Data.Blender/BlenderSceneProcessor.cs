using System.Diagnostics;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneProcessor : ISceneProcessor
    {
        public void RunSceneProcess( string command, string args ) { Process.Start( command, args ); }
    }
}