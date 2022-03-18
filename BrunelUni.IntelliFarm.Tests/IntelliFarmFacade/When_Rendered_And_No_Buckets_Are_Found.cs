using System.Net;
using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.IntelliFarmFacade
{
    public class When_Rendered_And_No_Buckets_Are_Found : Given_An_IntelliFarm_Facade
    {
        private Result _result;

        protected override void When( )
        {
            MockWebClient.Get( Arg.Any<string>( ) )
                .Returns( new WebDto
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = "buckets not found"
                } );
            _result = SUT.Render( "", "" );
        }

        [ Test ]
        public void Then_Failiure_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result );
        }
    }
}