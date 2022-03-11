using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneCommandsFacadeTests.SetSceneData
{
    public class When_Updated_Successfully : Given_A_SceneCommandsFacade
    {
        private const string BlendFile = "C:\\path\\path\\test.blend";
        private RenderDataDto _data;
        private Result _result;

        protected override void When( )
        {
            _data = new RenderDataDto
            {
                MaxBounces = 4,
                Samples = 100
            };
            MockSceneProcessor.WriteTemp( Arg.Any<RenderDataDto>( ) )
                .Returns( Result.Success( ) );
            MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) )
                .Returns( Result.Success( ) );
            MockRenderManagerService.RenderManager
                .GetRenderInfo( )
                .Returns( new RenderMetaDto
                {
                    BlendFilePath = BlendFile
                } );
            _result = SUT.SetSceneData( _data );
        }

        [ Test ]
        public void Then_Blender_Was_Run_Once( )
        {
            MockSceneProcessor.Received( 1 )
                .RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
        }

        [ Test ]
        public void Then_Correct_Blender_File_Was_Written_To( )
        {
            MockSceneProcessor.Received( ).RunSceneProcessAndExit( BlendFile, Arg.Any<string>( ), Arg.Any<bool>( ) );
        }

        [ Test ]
        public void Then_Blender_File_Was_Not_Rendered( )
        {
            MockSceneProcessor.Received( ).RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), false );
        }

        [ Test ]
        public void Then_Correct_Script_Was_Ran_With_Blender_File( )
        {
            MockSceneProcessor.Received( )
                .RunSceneProcessAndExit( Arg.Any<string>( ), "set_scene_data", Arg.Any<bool>( ) );
        }

        [ Test ]
        public void Then_Correct_Data_Was_Written_Into_Blender_File_And_File_Was_Cleared_Once( )
        {
            MockSceneProcessor.Received( 1 ).ClearTemp( );
            MockSceneProcessor.Received( 1 ).WriteTemp( Arg.Any<RenderDto>( ) );
            MockSceneProcessor.Received( ).WriteTemp( _data );
        }

        [ Test ]
        public void Then_Success_Was_Returned( ) { Assert.AreEqual( _result.Status, OperationResultEnum.Success ); }

        [ Test ]
        public void Then_Process_Is_Run_Before_Reading_Then_Cleared( )
        {
            Received.InOrder( ( ) =>
            {
                MockSceneProcessor.WriteTemp( Arg.Any<RenderDto>( ) );
                MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) );
                MockSceneProcessor.ClearTemp( );
            } );
        }
    }
}