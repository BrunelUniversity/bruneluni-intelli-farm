using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.ReadTemp
{
    public class When_Read_Successfully : Given_A_SceneProcessor
    {
        private ObjectResult<string> _objectResult;
        private ObjectResult<RenderDto> _result;
        private RenderDataDto _renderDataDto;
        private const string ContentValue = "test";

        protected override void When( )
        {
            _renderDataDto = new RenderDataDto { Samples = 100, MaxLightBounces = 4 };
            MockSerializer.Deserialize<object>( Arg.Any<string>( ) )
                .Returns( _renderDataDto );
            _objectResult = new ObjectResult<string> { Status = OperationResultEnum.Success, Value = ContentValue };
            MockFileAdapter.ReadFile( Arg.Any<string>( ) )
                .Returns( _objectResult );
            _result = SUT.ReadTemp( );
        }

        [ Test ]
        public void Then_Correct_File_Was_Read( )
        {
            MockSerializer.Received( 1 ).Deserialize<object>( Arg.Any<string>( ) );
            MockSerializer.Received( ).Deserialize<RenderDataDto>( ContentValue );
            MockFileAdapter.Received( 1 ).ReadFile( Arg.Any<string>( ) );
            MockFileAdapter.Received( ).ReadFile( TestConstants.DataScriptsTempFile );
        }

        [ Test ]
        public void Then_Blender_Data_Was_Extracted( )
        {
            MockProcessor.Received( 1 ).RunAndWait( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockProcessor.Received( ).RunAndWait( "blender", $"-b -P {TestConstants.ScriptDir}\\render_reader.py" );
        }

        [ Test ]
        public void Then_Data_Was_Returned_Successfully( )
        {
            Assert.AreEqual( _renderDataDto, _result.Value );
            Assert.AreEqual( OperationResultEnum.Success, _result.Status );
        }
    }
}