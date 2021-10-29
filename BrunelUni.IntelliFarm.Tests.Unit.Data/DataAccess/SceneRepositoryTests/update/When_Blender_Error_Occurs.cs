using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneRepositoryTests.update
{
    public class When_Blender_Error_Occurs : Given_A_SceneRepository
    {
        private Result _result;
        private const string BlenderErrorToBubbleUp = "blender error to bubble up";

        protected override void When( )
        {
            MockFileAdapter.WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            MockProcessor.RunAndWait( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Error( BlenderErrorToBubbleUp ) );
            _result = SUT.Update( new RenderDataDto( ) );
        }
        
        [ Test ]
        public void Then_Specific_Message_Is_Returned( )
        {
            Assert.AreEqual( BlenderErrorToBubbleUp, _result.Msg );
        }
        
        [ Test ]
        public void Then_Failiure_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
        }
    }
}