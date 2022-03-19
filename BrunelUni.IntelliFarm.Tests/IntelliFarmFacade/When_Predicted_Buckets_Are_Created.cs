using System.Collections.Generic;
using System.Linq;
using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Enums;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.IntelliFarmFacade
{
    public class When_Predicted_Buckets_Are_Created : Given_An_IntelliFarm_Facade
    {
        private string _someS3Key;
        private SceneDto _scene;
        private string _device1;
        private string _device2;
        private string _someZipPath;
        private string _someBlendPath;
        private List<ClientDto> _stateClients;
        private int _frame1;
        private int _frame2;
        private int _frame3;
        private BucketDto [ ] _buckets;

        protected override void When( )
        {
            MockFileAdapter.GetCurrentDirectory( ).Returns( new ObjectResult<string>
            {
                Value = "C:\\Dir\\Dir"
            } );
            _frame1 = 5;
            _frame2 = 6;
            _frame3 = 7;
            _device1 = "WEY-LAPTOP-1";
            _device2 = "WEY-LAPTOP-2";
            _someS3Key = "intellifarm/scenes/scene.zip";
            _someZipPath = "C:\\Dir\\Dir\\scene.zip";
            _someBlendPath = "C:\\Dir\\Dir\\scene.blend";
            MockRemoteFileService.DownloadFile( Arg.Any<string>( ) )
                .Returns( _someZipPath );
            _stateClients = new List<ClientDto>
            {
                new ClientDto
                {
                    Name = _device1,
                    TimeFor0PolyViewpoint = 7.4,
                    TimeFor80Poly100Coverage0Bounces100Samples = 20.4
                },
                new ClientDto
                {
                    Name = _device2,
                    TimeFor0PolyViewpoint = 9.4,
                    TimeFor80Poly100Coverage0Bounces100Samples = 29.3
                }
            };
            State.Clients = _stateClients;
            _buckets = new [ ]
            {
                new BucketDto
                {
                    DeviceId = _stateClients[ 0 ].Id,
                    Frames = new List<FrameTimeDto>
                    {
                        new FrameTimeDto
                        {
                            Num = 7,
                            Time = 4.4
                        }
                    }

                },
                new BucketDto
                {
                    DeviceId = _stateClients[ 1 ].Id,
                    Frames = new List<FrameTimeDto>
                    {
                        new FrameTimeDto
                        {
                            Num = 5,
                            Time = 5.5
                        },
                        new FrameTimeDto
                        {
                            Num = 6,
                            Time = 1.1
                        }
                    }
                }
            };
            MockRenderAnalyser.GetBuckets( Arg.Any<ClientDto [ ]>( ), Arg.Any<FrameDto [ ]>( ) )
                .Returns( _buckets );
            MockRemoteFileService.DownloadFile( Arg.Any<string>( ) )
                .Returns( _someZipPath );
            _scene = new SceneDto
            {
                Frames = new []
                {
                    new FrameDto
                    {
                        Number = _frame1
                    },
                    new FrameDto
                    {
                        Number = _frame2
                    },
                    new FrameDto
                    {
                        Number = _frame3
                    }
                },
                FileName = _someS3Key,
                Clients = new []
                {
                    new ClientDto
                    {
                        Name = _device1
                    },
                    new ClientDto
                    {
                        Name = _device2
                    }
                },
                StartFrame = 5
            };
            SUT.CreateBucketsFromProject( _scene );
        }

        [ Test ]
        public void Then_Commands_Occur_In_Order( )
        {
            Received.InOrder( ( ) =>
            {
                MockAnimationContext.Initialize( );
                MockAnimationContext.InitializeScene( _someBlendPath );
                
                MockSceneCommandFacade.SetSceneData( Arg.Is<RenderDataDto>( r =>
                    r.StartFrame == _scene.StartFrame &&
                    r.EndFrame == _scene.StartFrame ) );
                MockSceneCommandFacade.GetSceneData( );
                MockSceneCommandFacade.GetTriangleCount( );
                MockSceneCommandFacade.GetSceneAndViewportCoverage(
                    Arg.Is<RayCoverageInputDto>( r => r.Subdivisions == 8 ) );
                
                MockSceneCommandFacade.SetSceneData( Arg.Is<RenderDataDto>( r =>
                    r.StartFrame == _scene.StartFrame + 1 &&
                    r.EndFrame == _scene.StartFrame + 1 ) );
                MockSceneCommandFacade.GetSceneData( );
                MockSceneCommandFacade.GetTriangleCount( );
                MockSceneCommandFacade.GetSceneAndViewportCoverage(
                    Arg.Is<RayCoverageInputDto>( r => r.Subdivisions == 8 ) );
                
                MockSceneCommandFacade.SetSceneData( Arg.Is<RenderDataDto>( r =>
                    r.StartFrame == _scene.StartFrame + 2 &&
                    r.EndFrame == _scene.StartFrame + 2 ) );
                MockSceneCommandFacade.GetSceneData( );
                MockSceneCommandFacade.GetTriangleCount( );
                MockSceneCommandFacade.GetSceneAndViewportCoverage(
                    Arg.Is<RayCoverageInputDto>( r => r.Subdivisions == 8 ) );
            } );
        }

        [ Test ]
        public void Then_State_Is_Equal_To_Generated_Buckets( )
        {
            Assert.True( State.Buckets.Select( x => x.Type ).SequenceEqual( new [ ]
            {
                BucketTypeEnum.Predicted,
                BucketTypeEnum.Predicted,
                BucketTypeEnum.Predicted
            } ) );
            Assert.True( State.Buckets.Select( x => x.FilePath ).SequenceEqual( new [ ]
            {
                _someS3Key,
                _someS3Key,
                _someS3Key
            } ) );
            Assert.True( State.Buckets.Select( x => x.DeviceId ).SequenceEqual( new [ ]
            {
                _stateClients[0].Id,
                _stateClients[1].Id,
                _stateClients[2].Id
            } ) );
            Assert.True( State.Buckets.Select( x => x.SceneId ).SequenceEqual( new [ ]
            {
                _scene.Id,
                _scene.Id,
                _scene.Id
            } ) );
        }

        [ Test ]
        public void Then_File_Was_Downloaded( )
        {
            MockRemoteFileService.DownloadFile( _someS3Key );
        }
        
        [ Test ]
        public void Then_File_Was_Unzipped( )
        {
            MockZipAdapter.ExtractToDirectory( _someZipPath, _someBlendPath );
        }
    }
}