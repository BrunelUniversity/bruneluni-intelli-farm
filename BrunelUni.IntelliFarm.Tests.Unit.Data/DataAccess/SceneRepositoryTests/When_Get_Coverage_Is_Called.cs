using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneRepositoryTests
{
    public class When_Get_Coverage_Is_Called : Given_A_SceneRepository
    {
        private string _blendFilePath;
        private RayCoverageInputDto _rayCoverageInputDto;
        private RayCoverageResultDto _rayCoverageResultDto;
        private RayCoverageResultDto _result;

        protected override void When( )
        {
            _blendFilePath = "test//path";
            MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) )
                .Returns( Result.Success( ) );
            MockRenderManagerService.RenderManager.GetRenderInfo( ).Returns( new RenderMetaDto
            {
                BlendFilePath = _blendFilePath
            } );
            MockSceneProcessor
                .WriteTemp( Arg.Any<RayCoverageInputDto>( ) )
                .Returns( Result.Success( ) );
            _rayCoverageResultDto = new RayCoverageResultDto
            {
                Percentage = 50.0
            };
            MockSceneProcessor
                .ReadTemp<RayCoverageResultDto>( )
                .Returns( new ObjectResult<RayCoverageResultDto>
                {
                    Status = OperationResultEnum.Success,
                    Value = _rayCoverageResultDto
                } );
            _rayCoverageInputDto = new RayCoverageInputDto
            {
                Subdivisions = 4
            };
            _result = SUT.GetCoverage( _rayCoverageInputDto );
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
                .RunSceneProcessAndExit( _blendFilePath, "get_scene_and_viewpoint_coverage", false );
        }
    }
}