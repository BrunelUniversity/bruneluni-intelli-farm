using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.Initialize
{
    public class When_File_Format_Is_Invalid : Given_A_BlenderAnimationContext
    {
        private Result _result;

        protected override void When( )
        {
            MockFileAdapter.Exists( Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            MockFileAdapter.GetFileExtension( Arg.Any<string>( ) )
                .Returns( new ObjectResult<string>
                {
                    Status = OperationResultEnum.Success,
                    Value = ".txt"
                } );
            _result = SUT.Initialize( "" );
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