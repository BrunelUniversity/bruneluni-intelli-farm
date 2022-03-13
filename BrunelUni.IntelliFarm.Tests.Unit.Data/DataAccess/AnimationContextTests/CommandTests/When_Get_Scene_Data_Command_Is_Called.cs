using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.CommandTests
{
    public class When_Get_Scene_Data_Command_Is_Called : Given_A_CommandFacade
    {
        private RenderDataDto _dataOut;
        private RenderDataDto _result;

        protected override void When( )
        {
            _dataOut = new RenderDataDto( );
            MockCommandOut.Run<RenderDataDto>( ).Returns( _dataOut );
            _result = SUT.GetSceneData( );
        }

        [ Test ]
        public void Then_Out_Command_Is_Created( )
        {
            MockCommandOutFactory.Received( 1 ).Factory( Arg.Any<CommandMetaDto>( ) );
            MockCommandOutFactory
                .Received( )
                .Factory( Arg.Is<CommandMetaDto>( c => AssertMeta( c, "get_scene_data", false ) ) );
        }

        [ Test ]
        public void Then_Out_Command_Is_Ran( ) { MockCommandOut.Received( 1 ).Run<RenderDataDto>( ); }

        [ Test ]
        public void Then_Correct_Data_Is_Returned( ) { Assert.AreSame( _dataOut, _result ); }
    }
}