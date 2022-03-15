using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[ CheckBuildProjectConfigurations ]
[ ShutdownDotNetAfterServerBuild ]
class Build : NukeBuild
{
    [ Parameter ] readonly Configuration Configuration = Configuration.Debug;

    [ Solution ] readonly Solution Solution;

    Target Clean => _ => _
        .Executes( ( ) =>
        {
        } );

    Target Restore => _ => _
        .DependsOn( Clean )
        .Executes( ( ) =>
        {
            DotNetRestore( s => s
                .SetProjectFile( Solution ) );
        } );

    Target Compile => _ => _
        .DependsOn( Restore )
        .Executes( ( ) =>
        {
            DotNetBuild( s => s
                .SetProjectFile( Solution )
                .SetConfiguration( Configuration )
                .EnableNoRestore( ) );
        } );

    Target Test => _ => _
        .DependsOn( Compile )
        .Executes( ( ) =>
        {
            DotNetTest( s => s
                .SetVerbosity( DotNetVerbosity.Detailed )
                .SetProjectFile( Solution ) );
        } );

    public static int Main( ) => Execute<Build>( x => x.Test );
}