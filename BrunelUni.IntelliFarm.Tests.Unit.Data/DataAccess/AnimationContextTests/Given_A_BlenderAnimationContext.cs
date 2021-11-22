using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Fakes;
using NSubstitute;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests
{
    public abstract class Given_A_BlenderAnimationContext : GivenWhenThen<IAnimationContext>
    {
        protected IRenderManagerService MockRenderManagerService;
        protected IRenderManagerFactory MockRenderManagerFactory;
        protected IFileAdapter MockFileAdapter;
        protected IZipAdapter MockZipAdapter;
        private IScriptsRootDirectoryState _fakeScriptsRootDirectoryState;
        protected IWebClientAdapter MockWebClientAdapter;

        protected override void Given( )
        {
            MockRenderManagerService = Substitute.For<IRenderManagerService>( );
            MockRenderManagerFactory = Substitute.For<IRenderManagerFactory>( );
            MockFileAdapter = Substitute.For<IFileAdapter>( );
            MockZipAdapter = Substitute.For<IZipAdapter>( );
            _fakeScriptsRootDirectoryState = new FakeScriptsRootDirectoryState( );
            MockWebClientAdapter = Substitute.For<IWebClientAdapter>( );
            SUT = new BlenderAnimationContext( MockRenderManagerService, MockRenderManagerFactory, MockFileAdapter, MockZipAdapter, _fakeScriptsRootDirectoryState, MockWebClientAdapter );
        }
    }
}