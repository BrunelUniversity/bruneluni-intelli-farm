using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.EventRepositoryTests.Create
{
    public class When_Created_Successfully : Given_An_EventRepository
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
                .Returns( Result.Success(  ) );
            _result = SUT.Create( );
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
                .RunSceneProcessAndExit( "test", "logger", true );
        }
        
        [ Test ]
        public void Then_Temp_File_Is_Read_From( )
        {
            MockSceneProcessor
                .Received( 1 )
                .ReadTemp<RenderResultDto>( );
        }

        [ Test ]
        public void Then_Process_Is_Run_Before_Reading( )
        {
            Received.InOrder( ( ) =>
            {
                MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
                MockSceneProcessor.ReadTemp<RenderResultDto>( );
            } );
        }
    }
}