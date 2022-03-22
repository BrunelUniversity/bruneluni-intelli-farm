using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace BrunelUni.IntelliFarm.Build
{
    [ CheckBuildProjectConfigurations ]
    [ ShutdownDotNetAfterServerBuild ]
    class Build : NukeBuild
    {
        [ Parameter ] readonly Configuration Configuration = Configuration.Debug;

        [ Solution ] readonly Solution Solution;

        Target Test => _ => _
            .Executes( ( ) =>
            {
                DotNetTest( s => s
                    .SetProjectFile( Solution ) );
            } );

        public static int Main( ) => Execute<Build>( x => x.Test );
    }
}