using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.EventRepositoryTests.Create
{
    public class When_Created_Successfully : Given_An_EventRepository
    {
        private Result _result;

        protected override void When( )
        {
            MockRenderManagerService.RenderManager
                .GetRenderInfo( )
                .Returns( new RenderMetaDto
                {
                    BlendFilePath = "test"
                } );
            MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) )
                .Returns( Result.Success(  ) );
            _result = SUT.Create( new RenderEventDto(  ) );
        }

        [ Test ]
        public void Then_Success_Is_Returned( )
        {
        }
    }
}