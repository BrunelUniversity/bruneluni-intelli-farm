using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Constants;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.Initialize
{
    public class When_Initialized_Successuflly_If_Blender_Is_Installed_Already : Given_A_BlenderAnimationContext
    {
        protected override void When( )
        {
            MockZipAdapter.ExtractToDirectory( Arg.Any<string>( ), Arg.Any<string>( ) ).Returns( Result.Success( ) );
            MockWebClientAdapter.DownloadFile( Arg.Any<string>( ), Arg.Any<string>( ) ).Returns( Result.Success( ) );
            MockFileAdapter.Exists( Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            SUT.Initialize( );
        }

        [ Test ]
        public void Then_File_Was_Not_Downloaded( )
        {
            MockWebClientAdapter.DidNotReceive( ).DownloadFile( Arg.Any<string>( ), Arg.Any<string>( ) );
        }

        [ Test ]
        public void Then_File_Is_Not_Unzipped( )
        {
            MockZipAdapter.DidNotReceive( ).ExtractToDirectory( Arg.Any<string>( ), Arg.Any<string>( ) );
        }

        [ Test ]
        public void Then_Python_Source_Files_Are_Bundled( )
        {
            MockPythonBundler.Received( 1 ).CopySources( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockPythonBundler.Received( ).CopySources( TestConstants.BlenderScriptsModulesDirectory,
                TestConstants.DataScriptsDir );
        }
    }
}