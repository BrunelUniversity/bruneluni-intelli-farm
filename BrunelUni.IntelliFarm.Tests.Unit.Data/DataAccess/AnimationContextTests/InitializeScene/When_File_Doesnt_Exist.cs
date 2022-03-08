using System;
using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.InitializeScene
{
    public class When_File_Doesnt_Exist : Given_A_BlenderAnimationContext
    {
        [ Test ]
        public void Then_Render_Manager_Is_Not_Created( )
        {
            MockFileAdapter.Exists( Arg.Any<string>( ) )
                .Returns( Result.Error( "" ) );
            Assert.Throws<ArgumentException>( ( ) => SUT.InitializeScene( "" ) );
            MockRenderManagerFactory
                .DidNotReceive( )
                .Factory( Arg.Any<RenderMetaDto>( ) );
        }
    }
}