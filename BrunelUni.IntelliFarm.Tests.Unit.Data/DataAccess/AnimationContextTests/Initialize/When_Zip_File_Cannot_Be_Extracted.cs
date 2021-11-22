using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.Initialize
{
    public class When_Zip_File_Cannot_Be_Extracted : Given_A_BlenderAnimationContext
    {
        private Result _result;
        private const string MessageToBubble = "message to bubble";

        protected override void When( )
        {
            MockFileAdapter.Exists( Arg.Any<string>( ) )
                .Returns( Result.Error( "" ) );
            MockWebClientAdapter.DownloadFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            MockZipAdapter.ExtractToDirectory( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Error( MessageToBubble ) );
            _result = SUT.Initialize( );
        }

        [ Test ]
        public void Then_Failiure_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
            Assert.AreEqual( MessageToBubble, _result.Msg );
        }
    }
}