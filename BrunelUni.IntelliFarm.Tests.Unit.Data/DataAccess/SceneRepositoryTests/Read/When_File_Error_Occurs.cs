using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneRepositoryTests.Read
{
    public class When_File_Error_Occurs : Given_A_SceneRepository
    {
        private ObjectResult<RenderDataDto> _result;
        private const string Message = "message";
        
        protected override void When( )
        {
            MockSceneProcessor.ReadTemp<RenderDataDto>( )
                .Returns( new ObjectResult<RenderDataDto>
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
            _result = SUT.Read( );
        }
        
        [ Test ]
        public void Then_Failiure_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
            Assert.AreEqual( Message, _result.Msg );
        }
    }
}