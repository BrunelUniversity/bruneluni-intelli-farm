using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Fakes;
using NSubstitute;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests
{
    public class Given_A_SceneProcessor : GivenWhenThen<ISceneProcessor>
    {
        private IScriptsRootDirectoryState _fakeScriptsRootDirectoryState;
        private ILoggerAdapter<ISceneProcessor> _loggerAdapter;
        protected IFileAdapter MockFileAdapter;
        protected IProcessor MockProcessor;
        protected ISerializer MockSerializer;

        protected override void Given( )
        {
            MockProcessor = Substitute.For<IProcessor>( );
            MockSerializer = Substitute.For<ISerializer>( );
            MockFileAdapter = Substitute.For<IFileAdapter>( );
            _loggerAdapter = Substitute.For<ILoggerAdapter<ISceneProcessor>>( );
            _fakeScriptsRootDirectoryState = new FakeScriptsRootDirectoryState( );
            SUT = new BlenderSceneProcessor( MockProcessor, MockSerializer, MockFileAdapter,
                _fakeScriptsRootDirectoryState, _loggerAdapter );
        }
    }
}