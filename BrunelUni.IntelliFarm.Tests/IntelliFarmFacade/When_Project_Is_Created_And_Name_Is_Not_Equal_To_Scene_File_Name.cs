using System.Net;
using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.IntelliFarmFacade
{
    public class When_Project_Is_Created_And_Name_Is_Not_Equal_To_Scene_File_Name : Given_An_IntelliFarm_Facade
    {
        private Result _result;

        protected override void When( )
        {
            MockSceneCommandFacade.GetSceneData( ).Returns( new RenderDataDto( ) );
            MockFileAdapter.GetCurrentDirectory( )
                .Returns( new ObjectResult<string>
                {
                    Value = ""
                } );
            MockWebClient
                .Create( Arg.Any<string>( ), Arg.Any<SceneDto>( ) )
                .Returns( new WebDto
                {
                    Data = "not found",
                    StatusCode = HttpStatusCode.NotFound
                } );
            _result = SUT.CreateProject( "name", "C:\\Path\\To\\File\\file.notblend", "", "" );
        }
        
        [ Test ]
        public void Then_Result_Is_Failiure( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
        }
    }
}