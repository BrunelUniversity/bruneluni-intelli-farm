using System.Collections.Generic;
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
    public class When_Rendered : Given_An_IntelliFarm_Facade
    {
        private string _someS3Key;
        private string _currentDir;
        private string _zipFileAbsolutePath;
        private string _sceneName;
        private string _deviceName;
        private int _frame1;
        private int _frame2;
        private int _frame3;
        private double _renderTime1;
        private double _renderTime2;
        private int _renderTime3;
        private BucketDto _bucket;
        private Result _result;

        protected override void When( )
        {
            _sceneName = "test_scene";
            _deviceName = "WEY-243";
            _someS3Key = $"/path/file.zip";
            _frame1 = 10;
            _frame2 = 4;
            _frame3 = 12;
            _bucket = new BucketDto
            {
                FilePath = _someS3Key,
                Frames = new List<FrameTimeDto>
                {
                    new FrameTimeDto
                    {
                        Num = _frame1
                    },
                    new FrameTimeDto
                    {
                        Num = _frame2
                    },
                    new FrameTimeDto
                    {
                        Num = _frame3
                    }
                }
            };
            MockWebClient.Get( Arg.Any<string>( ) )
                .Returns( new WebDto
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = _bucket
                } );
            _currentDir = "C:\\Current\\Dir";
            MockFileAdapter.GetCurrentDirectory( ).Returns( new ObjectResult<string>
            {
                Status = OperationResultEnum.Success,
                Value = _currentDir
            } );
            _zipFileAbsolutePath = $"{_currentDir}\\{_sceneName}.zip";
            MockWebClient.DownloadFile( Arg.Any<string>( ), Arg.Any<string>( ) )
                .Returns( _zipFileAbsolutePath );
            _renderTime1 = 20.4;
            _renderTime2 = 18.4;
            _renderTime3 = 17;
            MockSceneCommandFacade
                .Render( )
                .Returns( new RenderResultDto
                    {
                        RenderTime = _renderTime1
                    },
                    new RenderResultDto
                    {
                        RenderTime = _renderTime2
                    },
                    new RenderResultDto
                    {
                        RenderTime = _renderTime3
                    } );
            _result = SUT.Render( _sceneName, _deviceName );
        }

        [ Test ]
        public void Then_Correct_Bucket_Is_Fetched( )
        {
            MockWebClient.Get( $"bucket?sceneName={_sceneName}&device={_deviceName}" );
        }

        [ Test ]
        public void Then_Correct_Scene_Is_Downloaded( )
        {
            MockWebClient.DownloadFile( $"scene-file?key={_someS3Key}", $"{_sceneName}.zip" );
        }
        
        [ Test ]
        public void Then_Correct_File_Is_Unzipped( )
        {
            MockZipAdapter.ExtractToDirectory( $"{_currentDir}\\{_sceneName}.zip", _currentDir );
        }

        [ Test ]
        public void Then_Actual_Bucket_Is_Created( )
        {
            MockWebClient.Create( "bucket", Arg.Is<BucketDto>( t =>
                t.DeviceId == _bucket.DeviceId &&
                t.SceneId == _bucket.SceneId &&
                t.Type == BucketTypeEnum.Actual &&
                t.Frames[ 0 ].Num == _frame1 && t.Frames[ 0 ].Time.Equals( _renderTime1 ) &&
                t.Frames[ 1 ].Num == _frame2 && t.Frames[ 1 ].Time.Equals( _renderTime2 ) &&
                t.Frames[ 2 ].Num == _frame3 && t.Frames[ 2 ].Time.Equals( _renderTime3 ) ) );
        }

        [ Test ]
        public void Then_Commands_Are_Called_In_Order( )
        {
            Received.InOrder( ( ) =>
            {
                MockAnimationContext.Initialize(  );
                MockAnimationContext.InitializeScene( $"{_currentDir}\\{_sceneName}.blend" );
                MockSceneCommandFacade.SetSceneData( Arg.Is<RenderDataDto>( r =>
                    r.StartFrame == _frame1 &&
                    r.EndFrame == _frame1 ) );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.SetSceneData( Arg.Is<RenderDataDto>( r =>
                    r.StartFrame == _frame2
                    && r.EndFrame == _frame2 ) );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.SetSceneData( Arg.Is<RenderDataDto>( r =>
                    r.StartFrame == _frame3
                    && r.EndFrame == _frame3 ) );
                MockSceneCommandFacade.Render( );
            } );
        }
        
        [ Test ]
        public void Then_Success_Is_Returned( )
        {
            Assert.AreEqual( OperationResultEnum.Success, _result );
        }
    }
}