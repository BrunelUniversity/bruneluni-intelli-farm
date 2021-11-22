using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.EventRepositoryTests
{
    public class Given_An_EventRepository : GivenWhenThen<IRenderEventRepository>
    {
        protected IRenderManagerService MockRenderManagerService;
        protected ISceneProcessor MockSceneProcessor;

        protected override void Given( )
        {
            MockRenderManagerService = Substitute.For<IRenderManagerService>( );
            MockSceneProcessor = Substitute.For<ISceneProcessor>( );
            SUT = new BlenderRenderEventRepository( MockRenderManagerService, MockSceneProcessor );
        }
    }
}