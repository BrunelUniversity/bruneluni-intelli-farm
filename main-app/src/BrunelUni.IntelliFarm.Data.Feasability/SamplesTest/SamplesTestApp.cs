using System.Collections.Generic;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Feasability.SamplesTest
{
    public class SamplesTestApp : IRunnable
    {
        private readonly IAnimationContext _animationContext;
        private readonly IBatchService _batchService;
        private readonly IConfigurationAdapter _configurationAdapter;
        private readonly ICsvAdapterFactory _csvAdapterFactory;
        private readonly IFeasabilityRespository _feasabilityRespository;
        private readonly IFileAdapter _fileAdapter;
        private readonly ILoggerAdapter<SamplesTestApp> _loggerAdapter;
        private readonly SamplesState _samplesState;
        private readonly ISceneCommandFacade _sceneCommandFacade;

        public SamplesTestApp( ISceneCommandFacade sceneCommandFacade,
            IAnimationContext animationContext,
            ILoggerAdapter<SamplesTestApp> loggerAdapter,
            SamplesState samplesState,
            ICsvAdapterFactory csvAdapterFactory,
            IFileAdapter fileAdapter,
            IFeasabilityRespository feasabilityRespository,
            IConfigurationAdapter configurationAdapter,
            IBatchService batchService )
        {
            _sceneCommandFacade = sceneCommandFacade;
            _animationContext = animationContext;
            _loggerAdapter = loggerAdapter;
            _samplesState = samplesState;
            _csvAdapterFactory = csvAdapterFactory;
            _fileAdapter = fileAdapter;
            _feasabilityRespository = feasabilityRespository;
            _configurationAdapter = configurationAdapter;
            _batchService = batchService;
        }

        public void Run( )
        {
            var appOptions = _configurationAdapter.Get<TestAppOptions>( );
            _animationContext.Initialize( );
            var initialSamples = _samplesState.Samples.Value;
            var initialBounes = _samplesState.Bounces.Value;

            foreach( var file in _samplesState.Files )
            {
                _animationContext.InitializeScene( file.File );
                _sceneCommandFacade.GetSceneData( );

                for( var i = 0; i < _samplesState.Bounces.Iterations; i++ )
                {
                    for( var j = 0; j < _samplesState.Samples.Iterations; j++ )
                    {
                        _sceneCommandFacade.SetSceneData( new RenderDataDto
                        {
                            StartFrame = 1,
                            EndFrame = 1,
                            MaxBounces = _samplesState.Bounces.Value,
                            DiffuseBounces = _samplesState.Bounces.Value,
                            Samples = _samplesState.Samples.Value
                        } );

                        var renderResult = _sceneCommandFacade.Render( );

                        _loggerAdapter.LogInfo(
                            $"render time was {renderResult.RenderTime} seconds with samples of {_samplesState.Samples.Value} and bounces of {_samplesState.Bounces.Value}" );
                        _samplesState.RenderResults.Add( new RenderSamplesResultDto
                        {
                            Samples = _samplesState.Samples.Value,
                            RenderTime = renderResult.RenderTime
                        } );

                        var filePath = _fileAdapter.GetCurrentDirectory( ).Value;

                        if( appOptions.BatchModeOptions.BatchMode )
                        {
                            _feasabilityRespository.Create( new FeasabilityDto
                            {
                                Coverage = file.Coverage,
                                MaxBounces = _samplesState.Bounces.Value,
                                Samples = _samplesState.Samples.Value,
                                PolyCount = file.PolyCount,
                                RenderTimeSeconds = renderResult.RenderTime,
                                Device = appOptions.Device,
                                Session = _batchService.Session,
                                Clusters = 1
                            } );
                        }
                        else
                        {
                            _loggerAdapter.LogInfo( $"csv dir => {filePath}\\samples.csv" );
                            _csvAdapterFactory
                                .Factory( $"{filePath}\\{file.Id}_{_samplesState.Bounces.Value}.csv" )
                                .Write( _samplesState.RenderResults );
                        }

                        _samplesState.Samples.Value += _samplesState.Samples.Diff;
                    }

                    _samplesState.Bounces.Value += _samplesState.Bounces.Diff;
                    _samplesState.Samples.Value = initialSamples;
                    _samplesState.RenderResults = new List<RenderSamplesResultDto>( );
                }

                _samplesState.Bounces.Value = initialBounes;
            }
        }
    }
}