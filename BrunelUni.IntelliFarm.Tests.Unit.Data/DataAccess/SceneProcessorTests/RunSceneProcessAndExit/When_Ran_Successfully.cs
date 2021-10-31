using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.RunSceneProcessAndExit
{
    public class When_Ran_Successfully : Given_A_SceneProcessor
    {
        private const string BlendFile = "C:\\Users\\roboc\\asset_1.blend";
        private const string Script = "writer";
        private readonly bool _render;
        private Result _result;

        public When_Ran_Successfully( bool render ) { _render = render; }
        
        protected override void When( )
        {
            MockProcessor.RunAndWait( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            _result = SUT.RunSceneProcessAndExit( BlendFile, Script, false );
        }

        [ Test ]
        public void Then_Blender_Process_Is_Run_In_The_Backround_With_Correct_Script( )
        {
            MockProcessor.Received( 1 ).RunAndWait( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockProcessor.Received( ).RunAndWait( "blender", $"{BlendFile} -b -P render_{Script}.py" );
        }

        [ Test ]
        public void Then_Success_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Success, _result.Status );
        }
    }
}