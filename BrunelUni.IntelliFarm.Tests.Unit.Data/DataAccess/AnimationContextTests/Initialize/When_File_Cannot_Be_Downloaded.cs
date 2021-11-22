using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.Initialize
{
    public class When_File_Cannot_Be_Downloaded : Given_A_BlenderAnimationContext
    {
        private Result _result;
        private const string ErrorMsg = "error msg";

        protected override void When( )
        {
            MockFileAdapter.Exists( Arg.Any<string>( ) )
                .Returns( Result.Error( "" ) );
            MockWebClientAdapter.DownloadFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Error( ErrorMsg ) );
            _result = SUT.Initialize( );
        }

        [ Test ]
        public void Then_Zip_File_Is_Not_Extracted( )
        {
            MockZipAdapter.DidNotReceive( ).ExtractToDirectory( Arg.Any<string>( ), Arg.Any<string>( ) );
        }

        [ Test ]
        public void Then_Failiure_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
            Assert.AreEqual( ErrorMsg, _result.Msg );
        }
    }
}