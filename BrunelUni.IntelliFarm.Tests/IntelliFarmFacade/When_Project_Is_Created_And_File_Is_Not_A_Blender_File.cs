using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.IntelliFarmFacade
{
    public class When_Project_Is_Created_And_File_Is_Not_A_Blender_File : Given_An_IntelliFarm_Facade
    {
        private Result _result;

        protected override void When( )
        {
            _result = SUT.CreateProject( "name", "C:\\Path\\To\\File\\file.notblend", "", "" );
        }
        
        [ Test ]
        public void Then_Result_Is_Failiure( )
        {
            Assert.AreEqual( OperationResultEnum.Failed, _result.Status );
        }
    }
}