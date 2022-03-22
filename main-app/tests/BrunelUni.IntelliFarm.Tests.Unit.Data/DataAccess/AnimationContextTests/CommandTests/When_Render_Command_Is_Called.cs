using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.CommandTests
{
    public class When_Render_Command_Is_Called : Given_A_CommandFacade
    {
        private RenderResultDto _dataOut;
        private RenderResultDto _result;

        protected override void When( )
        {
            _dataOut = new RenderResultDto( );
            MockCommandOut.Run<RenderResultDto>( ).Returns( _dataOut );
            _result = SUT.Render( );
        }

        [ Test ]
        public void Then_Out_Command_Is_Created( )
        {
            MockCommandOutFactory.Received( 1 ).Factory( Arg.Any<CommandMetaDto>( ) );
            MockCommandOutFactory
                .Received( )
                .Factory( Arg.Is<CommandMetaDto>( c => AssertMeta( c,
                    "render_frame",
                    true ) ) );
        }

        [ Test ]
        public void Then_Out_Command_Is_Ran( ) { MockCommandOut.Received( 1 ).Run<RenderResultDto>( ); }

        [ Test ]
        public void Then_Correct_Data_Is_Returned( ) { Assert.AreSame( _dataOut, _result ); }
    }
}