using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneRepositoryTests
{
    public class Given_A_SceneRepository : GivenWhenThen<ISceneRepository>
    {
        protected IProcessor MockProcessor;
        protected ISerializer MockSerializer;
        protected IFileAdapter MockFileAdapter;

        protected override void Given( )
        {
            MockProcessor = Substitute.For<IProcessor>( );
            MockSerializer = Substitute.For<ISerializer>( );
            MockFileAdapter = Substitute.For<IFileAdapter>( );
            SUT = new BlenderSceneRepository( MockProcessor, MockSerializer, MockFileAdapter );
        }
    }
}