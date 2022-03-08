using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Constants;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneProcessorTests.ClearTemp
{
    public class When_Cleared : Given_A_SceneProcessor
    {
        protected override void When( )
        {
            MockFileAdapter.WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) ).Returns( new Result
            {
                Status = OperationResultEnum.Success
            } );
            SUT.ClearTemp( );
        }

        [ Test ]
        public void Then_Correct_File_Was_Read( )
        {
            MockFileAdapter.Received( 1 ).WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockFileAdapter.Received( ).WriteFile( TestConstants.DataScriptsTempFile, "{}" );
        }
    }
}