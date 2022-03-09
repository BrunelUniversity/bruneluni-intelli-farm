using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Constants;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.RunSceneProcessAndExit
{
    public class When_Scene_Is_Rendered : Given_A_SceneProcessor
    {
        protected override void When( )
        {
            MockProcessor.RunAndWait( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            SUT.RunSceneProcessAndExit( "test", "test", true );
        }

        [ Test ]
        public void Then_Render_Argument_Is_Passed_Into_Blender_CLI( )
        {
            MockProcessor.Received( 1 ).RunAndWait( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockProcessor.Received( ).RunAndWait( Arg.Any<string>( ),
                $"test -b -P {TestConstants.DataScriptsDir}\\main.py -a" );
        }
    }
}