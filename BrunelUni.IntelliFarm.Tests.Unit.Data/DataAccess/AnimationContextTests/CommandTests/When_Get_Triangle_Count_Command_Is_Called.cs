using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.CommandTests
{
    public class When_Get_Triangle_Count_Command_Is_Called : Given_A_CommandFacade
    {
        private TriangleCountDto _dataOut;
        private TriangleCountDto _result;

        protected override void When( )
        {
            _dataOut = new TriangleCountDto( );
            MockCommandOut.Run<TriangleCountDto>( ).Returns( _dataOut );
            _result = SUT.GetTriangleCount( );
        }

        [ Test ]
        public void Then_Out_Command_Is_Created( )
        {
            MockCommandOutFactory.Received( 1 ).Factory( Arg.Any<CommandMetaDto>( ) );
            MockCommandOutFactory
                .Received( )
                .Factory( Arg.Is<CommandMetaDto>( c => AssertMeta( c, "get_triangles_count", false ) ) );
        }

        [ Test ]
        public void Then_Out_Command_Is_Ran( ) { MockCommandOut.Received( 1 ).Run<TriangleCountDto>( ); }

        [ Test ]
        public void Then_Correct_Data_Is_Returned( ) { Assert.AreSame( _dataOut, _result ); }
    }
}