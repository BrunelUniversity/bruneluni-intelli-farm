using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneCommandsFacadeTests.Render
{
    public class When_Created_Successfully : Given_A_SceneCommandsFacade
    {
        private ObjectResult<RenderResultDto> _result;

        protected override void When( )
        {
            MockRenderManagerService.RenderManager
                .GetRenderInfo( )
                .Returns( new RenderMetaDto
                {
                    BlendFilePath = "test"
                } );
            MockSceneProcessor
                .ReadTemp<RenderResultDto>( )
                .Returns( new ObjectResult<RenderResultDto>
                {
                    Status = OperationResultEnum.Success,
                    Value = new RenderResultDto
                    {
                        RenderTime = 2.22
                    }
                } );
            MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) )
                .Returns( Result.Success( ) );
            _result = SUT.Render( );
        }

        [ Test ]
        public void Then_Success_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Success, _result.Status );
            Assert.AreEqual( 2.22, _result.Value.RenderTime );
        }

        [ Test ]
        public void Then_Blender_Is_Ran_With_Correct_Args( )
        {
            MockSceneProcessor
                .Received( 1 )
                .RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
            MockSceneProcessor
                .Received( )
                .RunSceneProcessAndExit( "test", "render_frame", true );
        }

        [ Test ]
        public void Then_Temp_File_Is_Read_From_And_Cleared( )
        {
            MockSceneProcessor.Received( 1 ).ClearTemp( );
            MockSceneProcessor
                .Received( 1 )
                .ReadTemp<RenderResultDto>( );
        }

        [ Test ]
        public void Then_Process_Is_Run_Before_Reading_Then_Cleared( )
        {
            Received.InOrder( ( ) =>
            {
                MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
                MockSceneProcessor.ReadTemp<RenderResultDto>( );
                MockSceneProcessor.ClearTemp( );
            } );
        }
    }
}