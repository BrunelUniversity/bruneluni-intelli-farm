using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.IntelliFarm.Tests.IntelliFarmFacade
{
    public class Given_An_IntelliFarm_Facade : GivenWhenThen<IIntelliFarmFacade>
    {
        protected IWebClient MockWebClient;
        protected IConfigurationAdapter MockConfigurationAdapter;
        protected IZipAdapter MockZipAdapter;
        protected ISceneCommandFacade MockSceneCommandFacade;
        protected IAnimationContext MockAnimationContext;
        protected IFileAdapter MockFileAdapter;
        private IState _state;

        protected override void Given( )
        {
            MockWebClient = Substitute.For<IWebClient>( );
            MockConfigurationAdapter = Substitute.For<IConfigurationAdapter>( );
            MockZipAdapter = Substitute.For<IZipAdapter>( );
            MockSceneCommandFacade = Substitute.For<ISceneCommandFacade>( );
            MockAnimationContext = Substitute.For<IAnimationContext>( );
            MockFileAdapter = Substitute.For<IFileAdapter>( );
            _state = Substitute.For<IState>( );
            SUT = new Domain.IntelliFarmFacade( MockWebClient, MockConfigurationAdapter, MockZipAdapter, MockSceneCommandFacade,
                MockAnimationContext, MockFileAdapter, _state );
        }
    }
}