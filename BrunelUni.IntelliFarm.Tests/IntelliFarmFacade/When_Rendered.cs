using System.Collections.Generic;
using System.Net;
using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.IntelliFarmFacade
{
    public class When_Rendered : Given_An_IntelliFarm_Facade
    {
        private string _someS3Key;
        private string _currentDir;
        private string _someZipFile;
        private string _zipFileAbsolutePath;
        private string _sceneName;
        private string _deviceName;

        protected override void When( )
        {
            _sceneName = "test_scene";
            _deviceName = "WEY-243";
            _someZipFile = "some.zip";
            _someS3Key = $"/path/file.zip";
            MockWebClient.Get( Arg.Any<string>( ) )
                .Returns( new WebDto
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = new [ ]
                    {
                        new BucketDto
                        {
                            FilePath = _someS3Key,
                            Frames = new List<FrameTimeDto>
                            {
                                new FrameTimeDto
                                {
                                    Num = 10
                                },
                                new FrameTimeDto
                                {
                                    Num = 4
                                },
                                new FrameTimeDto
                                {
                                    Num = 12
                                }
                            }
                        }
                    }
                } );
            _currentDir = "C:\\Current\\Dir";
            MockFileAdapter.GetCurrentDirectory( ).Returns( new ObjectResult<string>
            {
                Status = OperationResultEnum.Success,
                Value = _currentDir
            } );
            _zipFileAbsolutePath = $"{_currentDir}\\{_someZipFile}";
            MockWebClient.DownloadFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( _zipFileAbsolutePath );
            MockSceneCommandFacade
                .Render( )
                .Returns( new RenderResultDto
                    {
                        RenderTime = 20.4
                    },
                    new RenderResultDto
                    {
                        RenderTime = 18.4
                    },
                    new RenderResultDto
                    {
                        RenderTime = 17
                    } );
            SUT.Render( _sceneName, _deviceName );
        }

        [ Test ]
        public void Then_Commands_Are_Called_In_Order( )
        {
            Received.InOrder( ( ) =>
            {
                MockWebClient.DownloadFile( $"scene-file?key={_someS3Key}", "some.zip" );
                MockZipAdapter.ExtractToDirectory( _zipFileAbsolutePath, _currentDir );
                MockWebClient.Get( $"bucket?sceneName={_sceneName}&device={_deviceName}" );
                MockAnimationContext.Initialize(  );
                MockAnimationContext.InitializeScene( $"{_currentDir}\\{_sceneName}.blend" );
            } );
        }
    }
}