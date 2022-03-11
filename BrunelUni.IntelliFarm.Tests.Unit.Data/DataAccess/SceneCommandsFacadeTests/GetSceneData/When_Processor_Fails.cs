using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneCommandsFacadeTests.GetSceneData
{
    public class When_Processor_Fails : Given_A_SceneCommandsFacade
    {
        private const string Message = "message";
        private ObjectResult<RenderDataDto> _result;

        protected override void When( )
        {
            MockSceneProcessor.ReadTemp<RenderDataDto>( )
                .Returns( new ObjectResult<RenderDataDto>
                {
                    Status = OperationResultEnum.Success
                } );
            MockRenderManagerService
                .RenderManager
                .GetRenderInfo( )
                .Returns( new RenderMetaDto( ) );
            MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) )
                .Returns( Result.Error( Message ) );
            _result = SUT.GetSceneData( );
        }

        [ Test ]
        public void Then_Failiure_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
            Assert.AreEqual( Message, _result.Msg );
        }
    }
}