using System;
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
            _animationContext.InitializeScene( _samplesState.File );
            var result = _sceneRepository.Read( );
            if( result.Status == OperationResultEnum.Failed )
            {
                throw new Exception( "failed to communicate with blender" );
            }
            for( var i = 0; i < _samplesState.Iterations; i++ )
            {
                var dataResult = _sceneRepository.Update( new RenderDataDto
                {
                    StartFrame = 1,
                    EndFrame = 1,
                    MaxBounces = 4,
                    Samples = _samplesState.Samples
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

                _loggerAdapter.LogInfo( $"render time was {renderResult.Value.RenderTime} seconds with samples of {_samplesState.Samples}" );
                _samplesState.RenderResults.Add( new RenderSamplesResultDto
                {
                    Samples = _samplesState.Samples,
                    RenderTime = renderResult.Value.RenderTime
                });
                _samplesState.Samples += _samplesState.SamplesDiff;
            }

            var filePath = _fileAdapter.GetCurrentDirectory(  ).Value;
            _loggerAdapter.LogInfo( $"non-lib current dir => {filePath}\\samples.csv" );
            _csvAdapterFactory
                .Factory( $"{filePath}\\samples.csv" )
                .Write( _samplesState.RenderResults );
        }
    }
}