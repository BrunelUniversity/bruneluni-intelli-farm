using System;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Domain
{
    public class BlenderCyclesRenderAnalyser : IRenderAnalyser
    {
        public(Guid clientId, int [ ] frameNums) [ ] GetFrameNumberBatches(
            (int framenum, double predictedTime, Guid clientId) frameAnaylsis )
        {
            throw new NotImplementedException( );
        }

        public double GetPredictedTime( CallibrationDto callibrationDto, FrameMetaData frameData )
        {
            // poly calc
            var exactPolyRenderTimeFor100Sam100SceneCov100ViewCov0Bounces =
                callibrationDto.TimeFor80Poly100Coverage0Bounces100Samples;
            if( frameData.TriangleCount > 1280 )
            {
                var polyExpMultiplier =
                    callibrationDto.TimeFor80Poly100Coverage0Bounces100Samples / Math.Pow( 80, SceneHeuristicsConstants.PolyExp );
                exactPolyRenderTimeFor100Sam100SceneCov100ViewCov0Bounces =
                    polyExpMultiplier * Math.Pow( frameData.TriangleCount, SceneHeuristicsConstants.PolyExp );
            }

            // samples calc
            var samplesMultiplier = (double) frameData.Samples / 100;
            var exactPolyAndSamRenderTimeFor100SceneCov100ViewCov0Bounces =
                samplesMultiplier * exactPolyRenderTimeFor100Sam100SceneCov100ViewCov0Bounces;
            
            // scene coverage calc
            var bounceHmaxFor100Cov = exactPolyAndSamRenderTimeFor100SceneCov100ViewCov0Bounces * SceneHeuristicsConstants.BouncesHmaxLogisticRegressionFunctionMult;
            var bounceHMinExpMult = exactPolyAndSamRenderTimeFor100SceneCov100ViewCov0Bounces /
                              Math.Pow( 8, SceneHeuristicsConstants.BouncesAndCovHMinCalcExp );
            var bounceHMinExactCov = bounceHMinExpMult *
                                     Math.Pow( frameData.SceneCoverage * SceneHeuristicsConstants.SceneCoverageMultForHMinExpCalc,
                                         SceneHeuristicsConstants.BouncesAndCovHMinCalcExp );
            var bounceIndex = 8 - ( frameData.SceneCoverage * SceneHeuristicsConstants.SceneCoverageMultForHMinExpCalc );
            var bounceHMaxExactCov = bounceHmaxFor100Cov *
                                     Math.Pow( 9 - ( frameData.SceneCoverage * SceneHeuristicsConstants.SceneCoverageMultForHMinExpCalc ),
                                         SceneHeuristicsConstants.SceneCoverageMultForHMaxExpCalc );
            var baseBounceRate = SceneHeuristicsConstants.Cov100BounceRate;

            var bounceRate = baseBounceRate + ( bounceIndex * SceneHeuristicsConstants.BounceRateDiff );

            // bounces calc
            var lastValue = bounceHMinExactCov;
            for( var i = 0; i < frameData.MaxDiffuseBounces; i++ )
            {
                lastValue = lastValue * ( 1.0 + ( bounceRate * ( 1.0 - ( lastValue / bounceHMaxExactCov ) ) ) );
            }

            var viewportWorldSpace = 100 - frameData.ViewportCoverage;

            var viewportWorldTime = callibrationDto.TimeFor0PolyViewpoint * ( viewportWorldSpace / 100 );
            var viewportObjectsTime = lastValue * ( frameData.ViewportCoverage / 100 );

            return viewportWorldTime + viewportObjectsTime;
        }
    }
}