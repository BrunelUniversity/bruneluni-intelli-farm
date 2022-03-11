using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneCommandsFacadeTests
{
    public class When_Get_Coverage_Is_Called : Given_A_SceneCommandsFacade
    {
        private string _blendFilePath;
        private RayCoverageInputDto _rayCoverageInputDto;
        private RayCoverageResultDto _rayCoverageResultDto;
        private RayCoverageResultDto _result;

        protected override void When( )
        {
            _blendFilePath = "test//path";
            MockRenderManagerService.RenderManager.GetRenderInfo( ).Returns( new RenderMetaDto
            {
                BlendFilePath = _blendFilePath
            } );
            _rayCoverageResultDto = new RayCoverageResultDto
            {
                Scene = 50.0,
                Viewport = 25.0
            };
            MockSceneProcessor
                .ReadTemp<RayCoverageResultDto>( )
                .Returns( _rayCoverageResultDto );
            _rayCoverageInputDto = new RayCoverageInputDto
            {
                Subdivisions = 4
            };
            _result = SUT.GetSceneAndViewportCoverage( _rayCoverageInputDto );
        }

        [ Test ]
        public void Then_Temp_File_Was_Written_To_Once_Read_From_Once_And_Cleared_Once( )
        {
            Received.InOrder( ( ) =>
            {
                MockSceneProcessor.WriteTemp( _rayCoverageInputDto );
                MockSceneProcessor.ReadTemp<RayCoverageResultDto>( );
                MockSceneProcessor.ClearTemp( );
            } );
            MockSceneProcessor.Received( 1 ).WriteTemp( Arg.Any<RenderDto>( ) );
            MockSceneProcessor.Received( 1 ).ReadTemp<RayCoverageResultDto>( );
            MockSceneProcessor.Received( 1 ).ClearTemp( );
        }

        [ Test ]
        public void Then_Coverage_Result_Was_The_Same_As_The_Object_Read( )
        {
            Assert.AreSame( _rayCoverageResultDto, _result );
        }

        [ Test ]
        public void Then_Correct_Script_Was_Called( )
        {
            MockSceneProcessor.Received( )
                .RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
            MockSceneProcessor.Received( 1 )
                .RunSceneProcessAndExit( _blendFilePath, "get_scene_and_viewport_coverage", false );
        }
    }
}