using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.CommandTests
{
    public class When_Get_Viewport_And_Scene_Coverage_Command_Is_Called : Given_A_CommandFacade
    {
        private RayCoverageInputDto _dataIn;
        private RayCoverageResultDto _dataOut;
        private RayCoverageResultDto _result;

        protected override void When( )
        {
            _dataIn = new RayCoverageInputDto( );
            _dataOut = new RayCoverageResultDto( );
            MockCommandInAndOut
                .Run<RayCoverageInputDto, RayCoverageResultDto>( Arg.Any<RayCoverageInputDto>( ) )
                .Returns( _dataOut );
            _result = SUT.GetSceneAndViewportCoverage( _dataIn );
        }

        [ Test ]
        public void Then_Out_Command_Is_Created( )
        {
            MockCommandInAndOutFactory.Received( 1 ).Factory( Arg.Any<CommandMetaDto>( ) );
            MockCommandInAndOutFactory
                .Received( )
                .Factory( Arg.Is<CommandMetaDto>( c => AssertMeta( c, "get_scene_and_viewport_coverage", false ) ) );
        }

        [ Test ]
        public void Then_Out_Command_Is_Ran( )
        {
            MockCommandInAndOut.Received( 1 )
                .Run<RayCoverageInputDto, RayCoverageResultDto>( Arg.Any<RayCoverageInputDto>( ) );
            MockCommandInAndOut.Received( ).Run<RayCoverageInputDto, RayCoverageResultDto>( _dataIn );
        }

        [ Test ]
        public void Then_Correct_Data_Is_Returned( ) { Assert.AreSame( _dataOut, _result ); }
    }
}