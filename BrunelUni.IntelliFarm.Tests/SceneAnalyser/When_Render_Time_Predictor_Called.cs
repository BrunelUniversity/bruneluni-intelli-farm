using System;
using BrunelUni.IntelliFarm.Core.Dtos;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    [ TestFixture( 80, 100, 100.0, 3, 100.0, 269, 97, double.NaN ) ]
    [ TestFixture( 80, 100, 100.0, 10, 100.0, 416.0, 97, double.NaN ) ]
    [ TestFixture( 80, 100, 75.0, 10, 100.0, 203.98, 97, double.NaN ) ]
    [ TestFixture( 80, 100, 62.5, 10, 100.0, 154.0, 97, double.NaN ) ]
    [ TestFixture( 80, 100, 50, 3, 100.0, 118.0, 97, double.NaN ) ]
    [ TestFixture( 80, 100, 25, 5, 100.0, 93, 97, double.NaN ) ]
    [ TestFixture( 648, 65, 87.5, 5, 100.0, 186, 130.5, double.NaN ) ]
    [ TestFixture( 1310720, 50, 100, 5, 100.0, 291, 97, double.NaN ) ]
    [ TestFixture( 1310720, 50, 100, 10, 100.0, 357, 97, double.NaN ) ]
    public class When_Render_Time_Predictor_Called : Given_A_RenderAnalyser
    {
        private readonly int _polyCount;
        private readonly int _samples;
        private readonly double _coverage;
        private readonly int _bounces;
        private readonly double _viewportCoverage;
        private readonly double _actualRenderTime;
        private readonly double _poly80Cov100Sam100Time;
        private readonly double _cov0Sam100RenderTime;
        private double _predictedTime;

        public When_Render_Time_Predictor_Called( int polyCount,
            int samples,
            double coverage,
            int bounces,
            double viewportCoverage,
            double actualRenderTime,
            double poly80Cov100Sam100Time,
            double cov0Sam100RenderTime )
        {
            _polyCount = polyCount;
            _samples = samples;
            _coverage = coverage;
            _bounces = bounces;
            _viewportCoverage = viewportCoverage;
            _actualRenderTime = actualRenderTime;
            _poly80Cov100Sam100Time = poly80Cov100Sam100Time;
            _cov0Sam100RenderTime = cov0Sam100RenderTime;
        }

        protected override void When( )
        {
            _predictedTime = SUT.GetPredictedTime( new CallibrationDto
            {
                TimeFor0PolyViewpoint = _cov0Sam100RenderTime,
                TimeFor80Poly100Coverage0Bounces100Samples = _poly80Cov100Sam100Time
            },
            new FrameMetaData
            {
                MaxDiffuseBounces = _bounces,
                Samples = _samples,
                TriangleCount = _polyCount,
                SceneCoverage = _coverage,
                ViewportCoverage = _viewportCoverage
            } );
        }

        [ Test ]
        public void Then_Prediction_Is_Valid_With_Respect_To_20s_Tolerance( )
        {
            Console.WriteLine( $"predicted: {_predictedTime}");
            Console.WriteLine( $"actual: {_actualRenderTime}");
            Console.WriteLine( Math.Abs( _predictedTime - _actualRenderTime ) );
            Assert.LessOrEqual( Math.Abs( _predictedTime - _actualRenderTime ), 20 );
        }
    }
}