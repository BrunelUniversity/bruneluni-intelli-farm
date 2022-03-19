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
        private string _currentDir;
        private int _bounces1 = 10;
        private int _samples1 = 50;
        private int _bounces2 = 4;
        private int _samples2 = 40;
        private int _bounces3 = 3;
        private int _samples3 = 60;
        private int _tri1 = 1200;
        private int _tri2 = 1220;
        private int _tri3 = 1230;
        private double _scene1 = 50.0;
        private double _viewport1 = 60.0;
        private double _scene2 = 75.0;
        private double _viewport2  = 85.0;
        private double _scene3 = 50.5;
        private double _viewport3 = 50.7;

        protected override void When( )
        {
            _currentDir = "C:\\Dir\\Dir";
            MockFileAdapter.GetCurrentDirectory( ).Returns( new ObjectResult<string>
            {
                Value = _currentDir
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
            MockSceneCommandFacade.GetSceneData( ).Returns( new RenderDataDto
                {
                    DiffuseBounces = _bounces1,
                    Samples = _samples1
                },
                new RenderDataDto
                {
                    DiffuseBounces = _bounces2,
                    Samples = _samples2
                },
                new RenderDataDto
                {
                    DiffuseBounces = _bounces3,
                    Samples = _samples3
                } );
            MockSceneCommandFacade.GetTriangleCount( ).Returns( new TriangleCountDto
                {
                    Count = _tri1
                },
                new TriangleCountDto
                {
                    Count = _tri2
                },
                new TriangleCountDto
                {
                    Count = _tri3
                } );
            MockSceneCommandFacade.GetSceneAndViewportCoverage( Arg.Any<RayCoverageInputDto>( ) )
                .Returns( new RayCoverageResultDto
                    {
                        Scene = _scene1,
                        Viewport = _viewport1
                    },
                    new RayCoverageResultDto
                    {
                        Scene = _scene2,
                        Viewport = _viewport2
                    },
                    new RayCoverageResultDto
                    {
                        Scene = _scene3,
                        Viewport = _viewport3
                    } );
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
            MockRemoteFileService.Received( 1 ).DownloadFile( Arg.Any<string>( ) );
            MockRemoteFileService.DownloadFile( _someS3Key );
        }
        
        [ Test ]
        public void Then_File_Was_Unzipped( )
        {
            MockZipAdapter.Received( 1 ).ExtractToDirectory( Arg.Any<string>( ), Arg.Any<string>( ) );
            MockZipAdapter.Received().ExtractToDirectory( _someZipPath, _currentDir );
        }

        [ Test ]
        public void Then_Predictor_Was_Called_With_Correct_Frame_Params( )
        {
            MockRenderAnalyser.GetBuckets( Arg.Is<ClientDto [ ]>( c =>
                c.Select( x => x.TimeFor0PolyViewpoint )
                    .SequenceEqual( new [ ]
                    {
                        _stateClients[ 0 ].TimeFor0PolyViewpoint,
                        _stateClients[ 1 ].TimeFor0PolyViewpoint
                    } ) &&
                c.Select( x => x.Name ).SequenceEqual( new [ ]
                {
                    _device1,
                    _device2
                } ) &&
                c.Select( x => x.TimeFor80Poly100Coverage0Bounces100Samples ).SequenceEqual( new [ ]
                {
                    _stateClients[ 0 ].TimeFor80Poly100Coverage0Bounces100Samples,
                    _stateClients[ 1 ].TimeFor80Poly100Coverage0Bounces100Samples
                } ) ), Arg.Is<FrameDto [ ]>( f =>
                f.Select( x => x.Number ).SequenceEqual( new [ ]
                {
                    _frame1,
                    _frame2,
                    _frame3
                } ) &&
                f.Select( x => x.Samples ).SequenceEqual( new [ ]
                {
                    _samples1,
                    _samples2,
                    _samples3
                } ) &&
                f.Select( x => x.Scene ).SequenceEqual( new []
                {
                    _scene.Id,
                    _scene.Id,
                    _scene.Id
                } ) &&
                f.Select( x => x.SceneCoverage ).SequenceEqual( new [ ]
                {
                    _scene1,
                    _scene2,
                    _scene3
                } ) &&
                f.Select( x => x.TriangleCount ).SequenceEqual( new [ ]
                {
                    _tri1,
                    _tri2,
                    _tri3
                } ) &&
                f.Select( x => x.ViewportCoverage ).SequenceEqual( new [ ]
                {
                    _viewport1,
                    _viewport2,
                    _viewport3
                } )&&
                f.Select( x => x.MaxDiffuseBounces ).SequenceEqual( new [ ]
                {
                    _bounces1,
                    _bounces2,
                    _bounces3
                } )) );
        }
    }
}