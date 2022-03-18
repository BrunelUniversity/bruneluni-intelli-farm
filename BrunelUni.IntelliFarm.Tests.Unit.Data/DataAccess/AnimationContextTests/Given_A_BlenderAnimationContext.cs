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
        private IScriptsRootDirectoryState _fakeScriptsRootDirectoryState;
        protected IFileAdapter MockFileAdapter;
        protected IPythonBundler MockPythonBundler;
        protected IRenderManagerFactory MockRenderManagerFactory;
        protected IRenderManagerService MockRenderManagerService;
        protected IWebClientAdapter MockWebClientAdapter;
        protected IZipAdapter MockZipAdapter;
        protected IConfigurationAdapter MockConfigurationAdapter;

        protected override void Given( )
        {
            MockRenderManagerService = Substitute.For<IRenderManagerService>( );
            MockRenderManagerFactory = Substitute.For<IRenderManagerFactory>( );
            MockFileAdapter = Substitute.For<IFileAdapter>( );
            MockZipAdapter = Substitute.For<IZipAdapter>( );
            _fakeScriptsRootDirectoryState = new FakeScriptsRootDirectoryState( );
            MockWebClientAdapter = Substitute.For<IWebClientAdapter>( );
            MockPythonBundler = Substitute.For<IPythonBundler>( );
            MockConfigurationAdapter = Substitute.For<IConfigurationAdapter>( );
            SUT = new BlenderAnimationContext( MockRenderManagerService, MockRenderManagerFactory, MockFileAdapter,
                MockZipAdapter, _fakeScriptsRootDirectoryState, MockWebClientAdapter, MockPythonBundler, MockConfigurationAdapter );
        }
    }
}