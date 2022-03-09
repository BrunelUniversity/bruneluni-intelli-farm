using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Constants;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.RunSceneProcessAndExit
{
    public class When_Ran_Successfully : Given_A_SceneProcessor
    {
        private const string BlendFile = "C:\\Users\\roboc\\asset_1.blend";
        private const string Script = "writer";
        private Result _result;

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
            MockProcessor.Received( ).RunAndWait( TestConstants.BlenderDirectory,
                $"{BlendFile} -b -P {TestConstants.DataScriptsDir}\\render.py -- {Script}" );
        }

        [ Test ]
        public void Then_Success_Is_Returned( ) { Assert.AreEqual( OperationResultEnum.Success, _result.Status ); }
    }
}