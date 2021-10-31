using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneRepositoryTests
{
    public class Given_A_SceneRepository : GivenWhenThen<ISceneRepository>
    {
        protected ISceneProcessor MockSceneProcessor;
        protected IRenderManagerService MockRenderManagerService;

        protected override void Given( )
        {
            MockSceneProcessor = Substitute.For<ISceneProcessor>( );
            MockRenderManagerService = Substitute.For<IRenderManagerService>( );
            SUT = new BlenderSceneRepository( MockSceneProcessor, MockRenderManagerService );
        }
    }
}