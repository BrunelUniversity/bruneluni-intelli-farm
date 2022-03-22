using System;
using System.Net;
using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Enums;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.IntelliFarmFacade
{
    public class When_Project_Is_Created : Given_An_IntelliFarm_Facade
    {
        private string _projectName;
        private string _projectFilePath;
        private string _device1;
        private string _device2;
        private string _device3;
        private string _projectDir;
        private string _s3Key;
        private RenderDataDto _sceneData;
        private int _totalFrames;
        private int _startFrame;
        private Result _result;

        protected override void When( )
        {
            MockWebClient.Create( Arg.Any<string>( ), Arg.Any<SceneDto>( ) )
                .Returns( new WebDto
                {
                    Data = new SceneDto
                    {
                        
                    },
                    StatusCode = HttpStatusCode.OK
                } );
            _totalFrames = 447;
            _startFrame = 54;
            _sceneData = new RenderDataDto
            {
                StartFrame = _startFrame,
                EndFrame = 500
            };
            _projectName = "file";
            _projectDir = "C:\\Project\\File\\Dir";
            _projectFilePath = $"{_projectDir}\\file.blend";
            MockFileAdapter.GetCurrentDirectory( ).Returns( new ObjectResult<string>
            {
                Status = OperationResultEnum.Success, Value = _projectDir
            } );
            MockSceneCommandFacade
                .GetSceneData( )
                .Returns( _sceneData );
            _s3Key = $"intellifarm\\scenes\\{Guid.NewGuid(  )}.zip";
            MockWebClient
                .UploadFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( _s3Key );
            _result = SUT.CreateProject( _projectName,
                _projectFilePath,
                _device1,
                _device2,
                _device3 );
        }

        [ Test ]
        public void Then_File_Is_Zipped( )
        {
            MockZipService.Received( 1 ).Zip( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockZipService.Received( 1 ).Zip( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockZipService.Received( ).Zip( _projectFilePath, $"{_projectDir}\\{_projectName}.zip" );
        }

        [ Test ]
        public void Then_Commands_Are_Only_Called_Once( )
        {
            MockAnimationContext.Received( 1 ).Initialize( );
            MockAnimationContext.Received( 1 ).InitializeScene( Arg.Any<string>( ) );
            MockSceneCommandFacade.Received( 1 ).GetSceneData( );
        }
        
        [ Test ]
        public void Then_Commands_Are_Recieved_In_Order( )
        {
            Received.InOrder( ( ) =>
            {
                MockAnimationContext.Initialize( );
                MockAnimationContext.InitializeScene( _projectFilePath );
                MockSceneCommandFacade.GetSceneData( );
            } );
        }
        
        [ Test ]
        public void Then_File_Is_Uploaded( )
        {
            MockWebClient.Received( ).UploadFile( "upload-file", $"{_projectDir}\\{_projectName}.zip" );
        }
        
        [ Test ]
        public void Then_Request_To_Create_A_Project_Is_Sent( )
        {
            MockWebClient.Received( ).Create( "scene", Arg.Is<SceneDto>( x => AssertDto( x ) ) );
        }

        private bool AssertDto( SceneDto dto ) =>
            dto.Status == RenderStatusEnum.NotStarted &&
            dto.FileName == _s3Key &&
            dto.Frames.Length == _totalFrames &&
            dto.StartFrame == _startFrame &&
            dto.Clients[ 0 ].Name == _device1 &&
            dto.Clients[ 1 ].Name == _device2 &&
            dto.Clients[ 2 ].Name == _device3 &&
            dto.Clients.Length == 3;
        
        [ Test ]
        public void Then_Result_Is_Success( )
        {
            Assert.AreEqual( OperationResultEnum.Success, _result.Status );
        }
        
        // TODO: for debugging
        [ Test ]
        public void Then_Result_Message_Is_Null( )
        {
            Assert.AreEqual( null, _result.Msg );
        }
    }
}