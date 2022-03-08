using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.InitializeScene
{
    public class When_Initialised_Successfully : Given_A_BlenderAnimationContext
    {
        private const string ExamplePathTestBlend = "example/path/test.blend";
        private IRenderManager _renderManager;

        protected override void When( )
        {
            _renderManager = Substitute.For<IRenderManager>( );
            MockFileAdapter
                .Exists( Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            MockFileAdapter
                .GetFileExtension( Arg.Any<string>( ) )
                .Returns( new ObjectResult<string>
                {
                    Status = OperationResultEnum.Success,
                    Value = ".blend"
                } );
            MockRenderManagerFactory
                .Factory( Arg.Any<RenderMetaDto>( ) )
                .Returns( _renderManager );
            SUT.InitializeScene( ExamplePathTestBlend );
        }

        [ Test ]
        public void Then_Correct_Path_To_Blend_File_Was_Used( )
        {
            MockFileAdapter.Received( 1 ).Exists( Arg.Any<string>( ) );
            MockFileAdapter.Received( ).Exists( ExamplePathTestBlend );
            MockFileAdapter.Received( 1 ).GetFileExtension( Arg.Any<string>( ) );
            MockFileAdapter.Received( ).GetFileExtension( ExamplePathTestBlend );
        }

        [ Test ]
        public void Then_A_Render_Manager_Pointing_To_Animation_File_Is_Created( )
        {
            MockRenderManagerFactory
                .Received( 1 )
                .Factory( Arg.Any<RenderMetaDto>( ) );
            MockRenderManagerFactory
                .Received( )
                .Factory( Arg.Is<RenderMetaDto>( x => x.BlendFilePath == ExamplePathTestBlend ) );
            MockRenderManagerService
                .Received( 1 )
                .RenderManager = Arg.Any<IRenderManager>( );
            MockRenderManagerService
                .Received( )
                .RenderManager = _renderManager;
        }
    }
}