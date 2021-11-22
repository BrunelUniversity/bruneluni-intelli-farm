using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.InitializeScene
{
    public class When_File_Doesnt_Exist : Given_A_BlenderAnimationContext
    {
        private Result _result;

        protected override void When( )
        {
            MockFileAdapter.Exists( Arg.Any<string>( ) )
                .Returns( Result.Error( "" ) );
            _result = SUT.InitializeScene( "" );
        }

        [ Test ]
        public void Then_Failiure_Occurs( )
        {
            Assert.AreEqual( _result.Status, OperationResultEnum.Failed );
        }

        [ Test ]
        public void Then_Render_Manager_Is_Not_Created( )
        {
            MockRenderManagerFactory
                .DidNotReceive( )
                .Factory( Arg.Any<RenderMetaDto>( ) );
        }
    }
}