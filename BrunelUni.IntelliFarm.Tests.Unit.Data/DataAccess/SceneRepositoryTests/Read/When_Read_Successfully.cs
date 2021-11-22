using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneRepositoryTests.Read
{
    public class When_Read_Successfully : Given_A_SceneRepository
    {
        private ObjectResult<RenderDataDto> _result;

        protected override void When( )
        {
            MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) )
                .Returns( Result.Success( ) );
            MockRenderManagerService.RenderManager
                .GetRenderInfo( )
                .Returns( new RenderMetaDto
                {
                    BlendFilePath = "test"
                } );
            MockSceneProcessor.ReadTemp<RenderDataDto>( )
                .Returns( new ObjectResult<RenderDataDto>
                {
                    Status = OperationResultEnum.Success,
                    Value = new RenderDataDto
                    {
                        Samples = 100,
                        MaxBounces = 4
                    }
                } );
            _result = SUT.Read( );
        }

        [ Test ]
        public void Then_Success_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Success, _result.Status );
        }

        [ Test ]
        public void Then_Blender_Is_Ran_With_Correct_Args( )
        {
            MockSceneProcessor
                .Received( 1 )
                .RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
            MockSceneProcessor
                .Received( )
                .RunSceneProcessAndExit( "test", "reader", false );
        }
        
        [ Test ]
        public void Then_Temp_File_Is_Read_From( )
        {
            MockSceneProcessor
                .Received( 1 )
                .ReadTemp<RenderDataDto>( );
        }

        [ Test ]
        public void Then_Process_Is_Run_Before_Reading( )
        {
            Received.InOrder( ( ) =>
            {
                MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
                MockSceneProcessor.ReadTemp<RenderDataDto>( );
            } );
        }
    }
}