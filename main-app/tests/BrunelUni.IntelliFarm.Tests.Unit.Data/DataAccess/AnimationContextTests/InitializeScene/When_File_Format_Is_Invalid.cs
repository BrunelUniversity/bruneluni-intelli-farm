using System;
using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.InitializeScene
{
    public class When_File_Format_Is_Invalid : Given_A_BlenderAnimationContext
    {
        [ Test ]
        public void Then_Render_Manager_Is_Not_Created_And_Failiure_Occurs( )
        {
            //arrange
            MockFileAdapter.Exists( Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            MockFileAdapter.GetFileExtension( Arg.Any<string>( ) )
                .Returns( new ObjectResult<string>
                {
                    Status = OperationResultEnum.Success,
                    Value = ".txt"
                } );

            //act/assert
            Assert.Throws<ArgumentException>( ( ) => SUT.InitializeScene( "" ) );
            MockRenderManagerFactory
                .DidNotReceive( )
                .Factory( Arg.Any<RenderMetaDto>( ) );
        }
    }
}