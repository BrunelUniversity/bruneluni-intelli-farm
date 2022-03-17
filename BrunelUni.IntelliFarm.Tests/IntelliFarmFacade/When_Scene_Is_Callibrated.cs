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
            _av0SceneTime = 20.6;
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
                MockWebClient.DownloadFile( "calibration-files", _calibrationZipFile );
                MockZipAdapter.ExtractToDirectory( _calibrationZipFile, _currentDir );
                MockAnimationContext.Initialize(  );
                MockAnimationContext.InitializeScene( $"{_currentDir}\\poly_80_100_coverage.blend" );
                MockSceneCommandFacade.SetSceneData( Arg.Is<RenderDataDto>( r =>
                    r.Samples == 100 &&
                    r.DiffuseBounces == 0 &&
                    r.MaxBounces == 0 &&
                    r.EndFrame == 1 &&
                    r.StartFrame == 1 ) );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.Render( );
                MockAnimationContext.InitializeScene( $"{_currentDir}\\vewiport_0.blend" );
                MockSceneCommandFacade.SetSceneData( Arg.Is<RenderDataDto>( r =>
                    r.Samples == 100 &&
                    r.DiffuseBounces == 0 &&
                    r.MaxBounces == 0 &&
                    r.EndFrame == 1 &&
                    r.StartFrame == 1 ) );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.Render( );
                MockSceneCommandFacade.Render( );
                MockWebClient.Create( "device", Arg.Is<ClientDto>( t =>
                    t.Name == _name &&
                    t.TimeFor0PolyViewpoint.Equals( _av0SceneTime ) &&
                    t.TimeFor80Poly100Coverage0Bounces100Samples.Equals( _avBaseSceneTime ) ) );
            } );
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