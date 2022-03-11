using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneCommandsFacadeTests.Render
{
    public class When_File_Error_Occurs : Given_A_SceneCommandsFacade
    {
        private const string Message = "message";
        private ObjectResult<RenderResultDto> _result;

        protected override void When( )
        {
            MockSceneProcessor.ReadTemp<RenderResultDto>( )
                .Returns( new ObjectResult<RenderResultDto>
                {
                    Status = OperationResultEnum.Failed,
                    Msg = Message
                } );
            MockRenderManagerService
                .RenderManager
                .GetRenderInfo( )
                .Returns( new RenderMetaDto( ) );
            MockSceneProcessor.RunSceneProcessAndExit( Arg.Any<string>( ), Arg.Any<string>( ), Arg.Any<bool>( ) )
                .Returns( Result.Success( ) );
            _result = SUT.Render( );
        }

        [ Test ]
        public void Then_Failiure_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
            Assert.AreEqual( Message, _result.Msg );
        }
    }
}