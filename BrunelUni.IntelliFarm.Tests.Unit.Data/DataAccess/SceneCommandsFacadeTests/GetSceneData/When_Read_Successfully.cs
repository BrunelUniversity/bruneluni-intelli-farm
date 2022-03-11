using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneCommandsFacadeTests.GetSceneData
{
    public class When_Read_Successfully : Given_A_SceneCommandsFacade
    {
        private RenderDataDto _renderDataDto;
        private RenderDataDto _result;

        protected override void When( )
        {
            MockRenderManagerService.RenderManager
                .GetRenderInfo( )
                .Returns( new RenderMetaDto
                {
                    BlendFilePath = "test"
                } );
            _renderDataDto = new RenderDataDto
            {
                Samples = 100,
                MaxBounces = 4
            };
            MockSceneProcessor.ReadTemp<RenderDataDto>( )
                .Returns( _renderDataDto );
            _result = SUT.GetSceneData( );
        }

        [ Test ]
        public void Then_Blender_Is_Ran_With_Correct_Args( )
        {
            MockSceneProcessor
                .Received( 1 )
                .RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
            MockSceneProcessor
                .Received( )
                .RunSceneProcessAndExit( "test", "get_scene_data", false );
        }

        [ Test ]
        public void Then_Temp_File_Is_Read_From_And_Cleared( )
        {
            MockSceneProcessor
                .Received( 1 )
                .ReadTemp<RenderDataDto>( );
            MockSceneProcessor.Received( 1 ).ClearTemp( );
        }

        [ Test ]
        public void Then_Process_Is_Run_Before_Reading_Then_Cleared( )
        {
            Received.InOrder( ( ) =>
            {
                MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
                MockSceneProcessor.ReadTemp<RenderDataDto>( );
                MockSceneProcessor.ClearTemp( );
            } );
        }

        [ Test ]
        public void Then_Correct_Data_Was_Returned( ) { Assert.AreSame( _renderDataDto, _result ); }
    }
}