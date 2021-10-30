using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.WriteTemp
{
    public class When_Written_Successfully : Given_A_SceneProcessor
    {
        private Result _result;
        private RenderDataDto _renderDataDto;
        private const string ValueToWriteTo = "value to write to";

        protected override void When( )
        {
            MockFileAdapter.WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            MockSerializer.Serialize( Arg.Any<RenderDataDto>( ) )
                .Returns( ValueToWriteTo );
            _renderDataDto = new RenderDataDto( );
            _result = SUT.WriteTemp( _renderDataDto );
        }

        [ Test ]
        public void Then_Correct_Data_Is_Written( )
        {
            MockFileAdapter.Received( 1 ).WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockFileAdapter.Received( ).WriteFile( TestConstants.DataScriptsTempFile, ValueToWriteTo );
            MockSerializer.Received( 1 ).Serialize( Arg.Any<RenderDataDto>( ) );
            MockSerializer.Received( ).Serialize( _renderDataDto );
        }

        [ Test ]
        public void Then_Result_Is_Successful( )
        {
            Assert.AreEqual( OperationResultEnum.Success, _result.Status );
        }
    }
}