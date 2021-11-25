using System;
using System.Collections.Generic;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Tests.Feasability.Data.SamplesTest
{
    public class SamplesTestApp : IRunnable
    {
        private readonly ISceneRepository _sceneRepository;
        private readonly IRenderEventRepository _renderEventRepository;
        private readonly IAnimationContext _animationContext;
        private readonly ILoggerAdapter<SamplesTestApp> _loggerAdapter;
        private readonly SamplesState _samplesState;
        private readonly ICsvAdapterFactory _csvAdapterFactory;
        private readonly IFileAdapter _fileAdapter;

        public SamplesTestApp( ISceneRepository sceneRepository,
            IRenderEventRepository renderEventRepository,
            IAnimationContext animationContext,
            ILoggerAdapter<SamplesTestApp> loggerAdapter,
            SamplesState samplesState,
            ICsvAdapterFactory csvAdapterFactory,
            IFileAdapter fileAdapter )
        {
            _sceneRepository = sceneRepository;
            _renderEventRepository = renderEventRepository;
            _animationContext = animationContext;
            _loggerAdapter = loggerAdapter;
            _samplesState = samplesState;
            _csvAdapterFactory = csvAdapterFactory;
            _fileAdapter = fileAdapter;
        }

        public void Run( )
        {
            _animationContext.Initialize( );
            var initialSamples = _samplesState.Samples.Value;
            var initialBounes = _samplesState.Bounces.Value;

            foreach( var file in _samplesState.Files )
            {
                
                _animationContext.InitializeScene( file.File );
                var result = _sceneRepository.Read( );
                if( result.Status == OperationResultEnum.Failed )
                {
                    throw new Exception( "failed to communicate with blender" );
                }
                for( var i = 0; i < _samplesState.Bounces.Iterations; i++ )
                {
                    for( var j = 0; j < _samplesState.Samples.Iterations; j++ )
                    {
                        var dataResult = _sceneRepository.Update( new RenderDataDto
                        {
                            StartFrame = 1,
                            EndFrame = 1,
                            MaxBounces = _samplesState.Bounces.Value,
                            Samples = _samplesState.Samples.Value
                        } );
                        if( dataResult.Status == OperationResultEnum.Failed )
                        {
                            throw new Exception( "error occured with updating samples" );
                        }
                
                        var renderResult = _renderEventRepository.Create( );
                        if( renderResult.Status == OperationResultEnum.Failed )
                        {
                            throw new Exception( "error occured with rendering" );
                        }

                        _loggerAdapter.LogInfo( $"render time was {renderResult.Value.RenderTime} seconds with samples of {_samplesState.Samples.Value} and bounces of {_samplesState.Bounces.Value}" );
                        _samplesState.RenderResults.Add( new RenderSamplesResultDto
                        {
                            Samples = _samplesState.Samples.Value,
                            RenderTime = renderResult.Value.RenderTime
                        });
                        _samplesState.Samples.Value += _samplesState.Samples.Diff;
                
                        var filePath = _fileAdapter.GetCurrentDirectory(  ).Value;
                        _loggerAdapter.LogInfo( $"csv dir => {filePath}\\samples.csv" );
                        _csvAdapterFactory
                            .Factory( $"{filePath}\\{file.Id}_{_samplesState.Bounces.Value}.csv" )
                            .Write( _samplesState.RenderResults );
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