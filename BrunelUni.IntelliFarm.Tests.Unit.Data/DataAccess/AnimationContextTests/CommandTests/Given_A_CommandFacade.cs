using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Fakes;
using NSubstitute;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.CommandTests
{
    public class Given_A_CommandFacade : GivenWhenThen<ISceneCommandFacade>
    {
        private IRenderManagerService _renderManagerService;
        protected string BlendFile;
        protected ICommandIn MockCommandIn;
        protected ICommandInAndOut MockCommandInAndOut;
        protected ICommandInAndOutFactory MockCommandInAndOutFactory;
        protected ICommandInFactory MockCommandInFactory;
        protected ICommandOut MockCommandOut;
        protected ICommandOutFactory MockCommandOutFactory;
        protected IScriptsRootDirectoryState ScriptsRootDirectoryState;

        protected override void Given( )
        {
            MockCommandOut = Substitute.For<ICommandOut>( );
            MockCommandIn = Substitute.For<ICommandIn>( );
            MockCommandInAndOut = Substitute.For<ICommandInAndOut>( );
            MockCommandInFactory = Substitute.For<ICommandInFactory>( );
            MockCommandOutFactory = Substitute.For<ICommandOutFactory>( );
            MockCommandInAndOutFactory = Substitute.For<ICommandInAndOutFactory>( );
            MockCommandOutFactory
                .Factory( Arg.Any<CommandMetaDto>( ) )
                .Returns( MockCommandOut );
            MockCommandInFactory
                .Factory( Arg.Any<CommandMetaDto>( ) )
                .Returns( MockCommandIn );
            MockCommandInAndOutFactory
                .Factory( Arg.Any<CommandMetaDto>( ) )
                .Returns( MockCommandInAndOut );
            _renderManagerService = Substitute.For<IRenderManagerService>( );
            BlendFile = "C:\\Users\\blender.blend";
            _renderManagerService
                .RenderManager
                .GetRenderInfo( )
                .BlendFilePath
                .Returns( BlendFile );
            ScriptsRootDirectoryState = new FakeScriptsRootDirectoryState( );
            SUT = new BlenderSceneCommandFacade( MockCommandInFactory,
                _renderManagerService,
                MockCommandOutFactory,
                MockCommandInAndOutFactory,
                ScriptsRootDirectoryState );
        }
    }
}