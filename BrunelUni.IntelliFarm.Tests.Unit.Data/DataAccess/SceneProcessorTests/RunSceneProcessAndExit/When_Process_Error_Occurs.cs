using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.RunSceneProcessAndExit
{
    public class When_Process_Error_Occurs : Given_A_SceneProcessor
    {
        private Result _result;
        private const string Message = "message";

        protected override void When( )
        {
            MockProcessor.RunAndWait( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Error( Message ) );
            _result = SUT.RunSceneProcessAndExit( "", "", false );
        }

        [ Test ]
        public void Then_Error_Occurs( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
            Assert.AreEqual( Message, _result.Msg );
        }
    }
}