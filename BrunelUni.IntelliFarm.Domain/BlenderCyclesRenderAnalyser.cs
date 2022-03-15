using System;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Domain
{
    public class BlenderCyclesRenderAnalyser : IRenderAnalyser
    {
        private const double SceneCoverageMultForHMaxExpCalc = -0.74;
        private const double PolyExp = 0.06;
        private const double BouncesHmaxLogisticRegressionFunctionMult = 4.65;
        private const double BouncesAndCovHMinCalcExp = 0.19;
        private const double SceneCoverageMultForHMinExpCalc = 0.08;
        private const int HMaxRange = 9;
        private const double Cov100BounceRate = 0.5625;
        private const double BounceRateDiff = 0.1;

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
                    callibrationDto.TimeFor80Poly100Coverage0Bounces100Samples / Math.Pow( 80, PolyExp );
                exactPolyRenderTimeFor100Sam100SceneCov100ViewCov0Bounces =
                    polyExpMultiplier * Math.Pow( frameData.TriangleCount, PolyExp );
            }

            // samples calc
            var samplesMultiplier = (double) frameData.Samples / 100;
            var exactPolyAndSamRenderTimeFor100SceneCov100ViewCov0Bounces =
                samplesMultiplier * exactPolyRenderTimeFor100Sam100SceneCov100ViewCov0Bounces;
            
            // scene coverage calc
            var bounceHmaxFor100Cov = exactPolyAndSamRenderTimeFor100SceneCov100ViewCov0Bounces * BouncesHmaxLogisticRegressionFunctionMult;
            var bounceHMinExpMult = exactPolyAndSamRenderTimeFor100SceneCov100ViewCov0Bounces /
                              Math.Pow( 8, BouncesAndCovHMinCalcExp );
            var bounceHMinExactCov = bounceHMinExpMult *
                                     Math.Pow( frameData.SceneCoverage * SceneCoverageMultForHMinExpCalc,
                                         BouncesAndCovHMinCalcExp );
            var bounceIndex = 8 - ( frameData.SceneCoverage * SceneCoverageMultForHMinExpCalc );
            var bounceHMaxExactCov = bounceHmaxFor100Cov *
                                     Math.Pow( 9 - ( frameData.SceneCoverage * SceneCoverageMultForHMinExpCalc ),
                                         SceneCoverageMultForHMaxExpCalc );
            var baseBounceRate = Cov100BounceRate;

            var bounceRate = baseBounceRate + ( bounceIndex * BounceRateDiff );

            // bounces calc
            var lastValue = bounceHMinExactCov;
            for( var i = 0; i < frameData.MaxDiffuseBounces; i++ )
            {
                lastValue = lastValue * ( 1.0 + ( bounceRate * ( 1.0 - ( lastValue / bounceHMaxExactCov ) ) ) );
            }

            return lastValue;
        }
    }
}