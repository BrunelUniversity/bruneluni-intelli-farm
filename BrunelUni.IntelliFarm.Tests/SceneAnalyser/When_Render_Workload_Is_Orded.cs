using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    [ TestFixtureSource( nameof( WeyOrderFixture ) ) ]
    public class When_Render_Workload_Is_Orded : Given_A_RenderAnalyser
    {
        private readonly List<PredictorFixtureDto> _predictorFixtureDtos;
        private List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)> _results = new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)>();
        private List<( double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)> _orderedResults;

        public When_Render_Workload_Is_Orded( List<PredictorFixtureDto> predictorFixtureDtos )
        {
            _predictorFixtureDtos = predictorFixtureDtos;
        }

        protected override void When( )
        {
            foreach( var dto in _predictorFixtureDtos )
            {
                _results.Add( ( dto.ActualRenderTime, SUT.GetPredictedTime( FixtureHelper.GetWeyCalibrationData, dto.FrameMetaData ), dto ) );
            }

            _orderedResults = _results.OrderBy( x => x.predictedRenderTime ).ToList( );

        }

        [ Test ]
        public void Then_Ordered( )
        {
            foreach( var result in _orderedResults )
            {
                Console.WriteLine( result.actualRenderTime );
            }
        }
    }
}