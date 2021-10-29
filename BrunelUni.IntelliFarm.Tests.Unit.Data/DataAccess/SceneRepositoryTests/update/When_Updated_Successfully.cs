using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.SceneRepositoryTests.update
{
    public class When_Updated_Successfully : Given_A_SceneRepository
    {
        private const string DataString = "test";
        private Result _result;
        private RenderDataDto _data;
        private const string ScriptDir = "BrunelUni.IntelliFarm.Data.Scripts\\data_scripts";
        private static readonly string DataScriptsTempDir = $"{ScriptDir}\\temp\\render.json";

        protected override void When( )
        {
            _data = new RenderDataDto
            {
                MaxLightBounces = 4,
                Samples = 100
            };
            MockSerializer.Serialize( Arg.Any<object>( ) )
                .Returns( DataString );
            MockFileAdapter.WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            MockProcessor.RunAndWait( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( Result.Success( ) );
            _result = SUT.Update( _data );
        }

        [ Test ]
        public void Then_Correct_Blender_Data_Was_Written_To_Correct_File( )
        {
            MockFileAdapter.Received( 1 ).WriteFile( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockFileAdapter.Received().WriteFile( DataScriptsTempDir, DataString );
        }

        [ Test ]
        public void Then_Correct_Data_Was_Serialised( )
        {
            MockSerializer.Received( 1 ).Serialize( Arg.Any<object>( ) );
            MockSerializer.Received( ).Serialize( _data );
        }

        [ Test ]
        public void Then_Blender_File_Was_Written_To( )
        {
            MockProcessor.Received( 1 ).RunAndWait( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockProcessor.Received( ).RunAndWait( $"blender", $"-b -P {ScriptDir}\\render_writer.py" );
        }

        [ Test ]
        public void Then_Success_Was_Returned( )
        {
            Assert.AreEqual( _result.Status, OperationResultEnum.Success );
        }
    }
}