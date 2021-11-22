using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.Initialize
{
    public class When_Initialized_Successuflly_If_Blender_Is_Installed_Already : Given_A_BlenderAnimationContext
    {
        private Result _result;

        protected override void When( )
        {
            MockFileAdapter.Exists( Arg.Any<string>( ) )
                .Returns( Result.Error( "" ) );
            _result = SUT.Initialize( );
        }
        
        [ Test ]
        public void Then_File_Was_Not_Downloaded( )
        {
            MockWebClientAdapter.DidNotReceive( ).DownloadFile( Arg.Any<string>( ), Arg.Any<string>( ) );
        }

        [ Test ]
        public void Then_File_Is_Not_Unzipped( )
        {
            MockZipAdapter.DidNotReceive( ).ExtractToDirectory( Arg.Any<string>( ), Arg.Any<string>( ) );
        }

        [ Test ]
        public void Then_Success_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Success, _result.Status );
        }
    }
}