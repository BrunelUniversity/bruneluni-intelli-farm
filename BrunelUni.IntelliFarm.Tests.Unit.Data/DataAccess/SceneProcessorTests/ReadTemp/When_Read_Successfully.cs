using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Constants;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.ReadTemp
{
    public class When_Read_Successfully : Given_A_SceneProcessor
    {
        private const string ContentValue = "test";
        private ObjectResult<string> _objectResult;
        private RenderDataDto _renderDataDto;
        private RenderDataDto _result;

        protected override void When( )
        {
            _renderDataDto = new RenderDataDto { Samples = 100, MaxBounces = 4 };
            MockSerializer.Deserialize<RenderDataDto>( Arg.Any<string>( ) )
                .Returns( _renderDataDto );
            _objectResult = new ObjectResult<string> { Status = OperationResultEnum.Success, Value = ContentValue };
            MockFileAdapter.ReadFile( Arg.Any<string>( ) )
                .Returns( _objectResult );
            _result = SUT.ReadTemp<RenderDataDto>( );
        }

        [ Test ]
        public void Then_Correct_File_Was_Read( )
        {
            MockSerializer.Received( 1 ).Deserialize<RenderDataDto>( Arg.Any<string>( ) );
            MockSerializer.Received( ).Deserialize<RenderDataDto>( ContentValue );
            MockFileAdapter.Received( 1 ).ReadFile( Arg.Any<string>( ) );
            MockFileAdapter.Received( ).ReadFile( TestConstants.DataScriptsTempFile );
        }

        [ Test ]
        public void Then_Correct_Data_Was_Serialized( )
        {
            MockSerializer.Received( 1 ).Deserialize<RenderDataDto>( Arg.Any<string>( ) );
            MockSerializer.Received( ).Deserialize<RenderDataDto>( _objectResult.Value );
        }

        [ Test ]
        public void Then_Correct_Data_Was_Returned( ) { Assert.AreSame( _renderDataDto, _result ); }
    }
}