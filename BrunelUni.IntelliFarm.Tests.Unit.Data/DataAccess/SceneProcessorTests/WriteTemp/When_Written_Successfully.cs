using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Constants;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.WriteTemp
{
    public class When_Written_Successfully : Given_A_SceneProcessor
    {
        private const string ValueToWriteTo = "value to write to";
        private RenderDataDto _renderDataDto;

        protected override void When( )
        {
            MockFileAdapter.WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            MockSerializer.Serialize( Arg.Any<RenderDataDto>( ) )
                .Returns( ValueToWriteTo );
            _renderDataDto = new RenderDataDto( );
            SUT.WriteTemp( _renderDataDto );
        }

        [ Test ]
        public void Then_Correct_Data_Is_Written( )
        {
            MockFileAdapter.Received( 1 ).WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockFileAdapter.Received( ).WriteFile( TestConstants.DataScriptsTempFile, ValueToWriteTo );
            MockSerializer.Received( 1 ).Serialize( Arg.Any<RenderDataDto>( ) );
            MockSerializer.Received( ).Serialize( _renderDataDto );
        }
    }
}