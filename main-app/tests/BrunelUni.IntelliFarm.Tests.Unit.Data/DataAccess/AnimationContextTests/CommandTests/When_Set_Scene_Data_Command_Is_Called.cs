using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.CommandTests
{
    public class When_Set_Scene_Data_Command_Is_Called : Given_A_CommandFacade
    {
        private RenderDataDto _dataIn;

        protected override void When( )
        {
            _dataIn = new RenderDataDto( );
            SUT.SetSceneData( _dataIn );
        }

        [ Test ]
        public void Then_In_Command_Is_Created( )
        {
            MockCommandInFactory.Received( 1 ).Factory( Arg.Any<CommandMetaDto>( ) );
            MockCommandInFactory
                .Received( )
                .Factory( Arg.Is<CommandMetaDto>( c => AssertMeta( c, "set_scene_data", false ) ) );
        }

        [ Test ]
        public void Then_Out_Command_Is_Ran( )
        {
            MockCommandIn.Received( ).Run( _dataIn );
            MockCommandIn.Received( 1 ).Run( Arg.Any<RenderDataDto>( ) );
        }
    }
}