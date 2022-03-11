using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneCommandsFacadeTests
{
    public class Given_A_SceneCommandsFacade : GivenWhenThen<ISceneCommandFacade>
    {
        private ILoggerAdapter<ISceneCommandFacade> _loggerAdapter;
        protected IRenderManagerService MockRenderManagerService;
        protected ISceneProcessor MockSceneProcessor;

        protected override void Given( )
        {
            MockSceneProcessor = Substitute.For<ISceneProcessor>( );
            MockRenderManagerService = Substitute.For<IRenderManagerService>( );
            _loggerAdapter = Substitute.For<ILoggerAdapter<ISceneCommandFacade>>( );
            SUT = new BlenderSceneCommandFacade( MockSceneProcessor, MockRenderManagerService, _loggerAdapter );
        }
    }
}