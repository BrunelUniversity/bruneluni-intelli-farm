using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneCommandsFacadeTests
{
    public class When_Get_Triangle_Count_Is_Called : Given_A_SceneCommandsFacade
    {
        private TriangleCountDto _result;
        private TriangleCountDto _triangleCountDto;

        protected override void When( )
        {
            MockRenderManagerService.RenderManager
                .GetRenderInfo( )
                .Returns( new RenderMetaDto
                {
                    BlendFilePath = "test"
                } );
            _triangleCountDto = new TriangleCountDto
            {
                Count = 16
            };
            MockSceneProcessor.ReadTemp<TriangleCountDto>( )
                .Returns( _triangleCountDto );
            _result = SUT.GetTriangleCount( );
        }

        [ Test ]
        public void Then_Blender_Is_Ran_With_Correct_Args( )
        {
            MockSceneProcessor
                .Received( 1 )
                .RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
            MockSceneProcessor
                .Received( )
                .RunSceneProcessAndExit( "test", "get_triangles_count", false );
        }

        [ Test ]
        public void Then_Temp_File_Is_Read_From_And_Cleared( )
        {
            MockSceneProcessor
                .Received( 1 )
                .ReadTemp<TriangleCountDto>( );
            MockSceneProcessor.Received( 1 ).ClearTemp( );
        }

        [ Test ]
        public void Then_Process_Is_Run_Before_Reading_Then_Cleared( )
        {
            Received.InOrder( ( ) =>
            {
                MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
                MockSceneProcessor.ReadTemp<TriangleCountDto>( );
                MockSceneProcessor.ClearTemp( );
            } );
        }

        [ Test ]
        public void Then_Correct_Data_Was_Returned( ) { Assert.AreSame( _triangleCountDto, _result ); }
    }
}