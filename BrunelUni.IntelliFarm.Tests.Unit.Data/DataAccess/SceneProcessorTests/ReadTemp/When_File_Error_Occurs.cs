using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.ReadTemp
{
    public class When_File_Error_Occurs : Given_A_SceneProcessor
    {
        private ObjectResult<RenderDataDto> _result;
        private const string Message = "bubble up";

        protected override void When( )
        {
            MockFileAdapter.ReadFile( Arg.Any<string>( ) ).Returns( new ObjectResult<string>
                { Status = OperationResultEnum.Failed, Msg = Message } );
            _result = SUT.ReadTemp<RenderDataDto>( );
        }

        [ Test ]
        public void Then_Failiure_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
            Assert.AreEqual( Message, _result.Msg );
        }
    }
}