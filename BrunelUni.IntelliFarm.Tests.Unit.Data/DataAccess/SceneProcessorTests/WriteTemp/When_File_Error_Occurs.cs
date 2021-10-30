using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.WriteTemp
{
    public class When_File_Error_Occurs : Given_A_SceneProcessor
    {
        private Result _result;
        private const string MessageToBubbleUp = "message to bubble up";

        protected override void When( )
        {
            MockFileAdapter.WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Error( MessageToBubbleUp ) );
            _result = SUT.WriteTemp( new RenderDataDto( ) );
        }

        [ Test ]
        public void Then_Failiure_Is_Returned( )
        {
            Assert.AreEqual( MessageToBubbleUp, _result.Msg );
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
        }
    }
}