using System;
using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.IntelliFarmFacade
{
    public class When_Scene_Is_Callibrated : Given_An_IntelliFarm_Facade
    {
        private string _name;
        private string _currentDir;
        private string _calibrationZipFile;
        private double _avBaseSceneTime;
        private double _av0SceneTime;

        protected override void When( )
        {
            _avBaseSceneTime = 18.6;
            _av0SceneTime = 6.86666666667;
            _calibrationZipFile = "calibration.zip";
            _name = "name";
            _currentDir = "C:\\Current\\Dir";
            MockFileAdapter.GetCurrentDirectory( ).Returns( new ObjectResult<string>
            {
                Status = OperationResultEnum.Success,
                Value = _currentDir
            } );
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
                },
                new RenderResultDto
                {
                    RenderTime = 7.6
                },
                new RenderResultDto
                {
                    RenderTime = 5
                },
                new RenderResultDto
                {
                    RenderTime = 8
                });
            SUT.CreateDevice( _name );
        }

        [ Test ]
        public void Then_Commands_Are_Recieved_In_Order( )
        {
            Received.InOrder( ( ) =>
            {
                MockAnimationContext.Initialize(  );
                MockAnimationContext.InitializeScene( Arg.Any<string>( ) );
                MockSceneCommandFacade.SetSceneData( Arg.Any<RenderDataDto>( ) );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.Render( );
                MockAnimationContext.InitializeScene( Arg.Any<string>( ) );
                MockSceneCommandFacade.SetSceneData( Arg.Any<RenderDataDto>( ) );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.Render( );
                MockWebClient.Create( Arg.Any<string>( ), Arg.Any<ClientDto>( ) );
            } );
        }

        private bool ClientDtoAssert( ClientDto clientDto ) =>
            clientDto.Name == _name &&
            Math.Round( clientDto.TimeFor0PolyViewpoint, 1 ).Equals( Math.Round( _av0SceneTime, 1 ) ) &&
            Math.Round( clientDto.TimeFor80Poly100Coverage0Bounces100Samples, 1 )
                .Equals( Math.Round( _avBaseSceneTime, 1 ) );

        [ Test ]
        public void Then_Correct_Data_Is_Fetched_From_Remote_File_Storage( )
        {
            MockWebClient.DownloadFile( "calibration-files", _calibrationZipFile );
            MockZipAdapter.ExtractToDirectory( _calibrationZipFile, _currentDir );
        }
        
        [ Test ]
        public void Then_Correct_Device_Is_Created( )
        {
            MockWebClient.Received( ).Create( "device", Arg.Is<ClientDto>( x => ClientDtoAssert( x ) ) );
        }

        [ Test ]
        public void Then_Correct_Scenes_Are_Initialized( )
        {
            MockAnimationContext.Received( ).InitializeScene( $"{_currentDir}\\poly_80_100_coverage.blend" );
            MockAnimationContext.Received( ).InitializeScene( $"{_currentDir}\\vewiport_0.blend" );
        }
        
        [ Test ]
        public void Then_Data_Is_Sent_To_Scenes( )
        {
            MockSceneCommandFacade.Received( 2 ).SetSceneData( Arg.Is<RenderDataDto>( r =>
                r.Samples == 100 &&
                r.DiffuseBounces == 0 &&
                r.MaxBounces == 0 &&
                r.EndFrame == 1 &&
                r.StartFrame == 1 ) );
        }
        
        [ Test ]
        public void Then_Commands_Are_Called_Correct_Number_Of_Times( )
        {
            MockAnimationContext.Received( 1 ).Initialize( );
            MockAnimationContext.Received( 2 ).InitializeScene( Arg.Any<string>( ) );
            MockSceneCommandFacade.Received( 6 ).Render( );
            MockWebClient.Received( 1 ).Create( Arg.Any<string>( ), Arg.Any<ClientDto>( ) );
        }
    }
}