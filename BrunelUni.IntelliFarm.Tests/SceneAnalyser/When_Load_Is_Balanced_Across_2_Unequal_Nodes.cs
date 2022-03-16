using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    [ TestFixtureSource( nameof( WeyOrderFixture2 ) ) ]
    public class When_Load_Is_Balanced_Across_2_Unequal_Nodes : Given_A_RenderAnalyser
    {
        private readonly List<PredictorFixtureDto> _predictorFixtureDtos;
        private List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)> _results = new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)>();
        private List<( double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)> _orderedResults;

        public When_Load_Is_Balanced_Across_2_Unequal_Nodes( List<PredictorFixtureDto> predictorFixtureDtos )
        {
            _predictorFixtureDtos = predictorFixtureDtos;
        }

        protected override void When( )
        {
            foreach( var dto in _predictorFixtureDtos )
            {
                _results.Add( ( SUT.GetPredictedTime( FixtureHelper.GetWeyCalibrationData, dto.FrameMetaData ), dto.ActualRenderTime, dto ) );
            }
            
            _orderedResults = _results.OrderByDescending( x => x.predictedRenderTime ).ToList( );
        }

        [ Test ]
        public void Then_The_Difference_In_Render_Time_Must_Be_Within_A_Given_Tolerance( )
        {
            // fastest == calculations are relative to speed of bucket 1 (fastest completing bucket)
            var bucket1Speed = 1;
            
            // slowest == calculations are relative to speed of bucket 1 (fastest completing bucket)
            var bucket2Speed = 0.5;
            var totalBucketSpeedDivisor = bucket1Speed + bucket2Speed;

            var bucket1Mult = bucket1Speed / totalBucketSpeedDivisor;
            var bucket2Mult = bucket2Speed / totalBucketSpeedDivisor;

            var totalTime = _orderedResults.Sum( x => x.predictedRenderTime );

            Console.WriteLine($"total time: {totalTime}");
            
            var time1Bucket = totalTime * bucket1Mult;
            var time2Bucket = totalTime * bucket2Mult;
            
            Console.WriteLine($"total time 1: {time1Bucket}");
            Console.WriteLine($"total time 2: {time2Bucket}");
            
            Console.WriteLine($"total time 1 normalized: {time1Bucket*1/bucket1Mult}");
            Console.WriteLine($"total time 2 normalized: {time2Bucket*1/bucket2Mult}");
            
            var bucket1 =
                new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef
                    )>( );
            var bucket2 =
                new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef
                    )>( );
            var count = 0;
            var bucket1Complete = false;
            var bucket2Complete = false;
            var totalOfWhole = _orderedResults.Count;
            var smallestValue = _orderedResults.Min( x => x.predictedRenderTime );
            while(_orderedResults.Any())
            {
                var result = _orderedResults[ 0 ];

                var bucket1Times = bucket1.Sum( x => x.predictedRenderTime );
                var bucket2Times = bucket2.Sum( x => x.predictedRenderTime );
                if( !bucket1Complete )
                {
                    if( bucket1Times + result.predictedRenderTime > time1Bucket )
                    {
                        foreach( var newResult in _orderedResults )
                        {
                            if( bucket1Times + newResult.predictedRenderTime > time1Bucket )
                            {
                                if( _orderedResults.IndexOf( newResult ) == _orderedResults.Count - 1 )
                                {
                                    bucket1.Add( newResult );
                                    _orderedResults.Remove( newResult );
                                    bucket1Complete = true;
                                    break;
                                }
                            }
                            else
                            {
                                bucket1.Add( newResult );
                                _orderedResults.Remove( newResult );
                                break;
                            }
                        }
                    }
                    else
                    {
                        bucket1.Add( result );
                        _orderedResults.Remove( result );
                    }
                }

                if( _orderedResults.Any( ) )
                {
                    result = _orderedResults[ 0 ];
                }
                else
                {
                    break;
                }

                if( !bucket2Complete )
                {
                    if( bucket2Times + result.predictedRenderTime > time2Bucket )
                    {
                        foreach( var newResult in _orderedResults )
                        {
                            if( bucket2Times + newResult.predictedRenderTime > time2Bucket )
                            {
                                if( _orderedResults.IndexOf( newResult ) == _orderedResults.Count - 1 )
                                {
                                    bucket2.Add( newResult );
                                    _orderedResults.Remove( newResult );
                                    bucket2Complete = true;
                                    break;
                                }
                            }
                            else
                            {
                                bucket2.Add( newResult );
                                _orderedResults.Remove( newResult );
                                break;
                            }
                        }
                    }
                    else
                    {
                        bucket2.Add( result );
                        _orderedResults.Remove( result );
                    }
                }
            }

            Console.WriteLine($"total between buckets: {bucket1.Count + bucket2.Count}, total of whole list: {totalOfWhole}");
            
            var predictedBucket1 = bucket1.Sum( x => x.predictedRenderTime );
            var predictedBucket2 = bucket2.Sum( x => x.predictedRenderTime );
            
            var actualBucket1 = bucket1.Sum( x => x.actualRenderTime );
            var actualBucket2 = bucket2.Sum( x => x.actualRenderTime );

            var predictedTotalTimeProportion1 = predictedBucket1 / (predictedBucket1+predictedBucket2);
            var predictedTotalTimeProportion2 = predictedBucket2 / (predictedBucket1+predictedBucket2);
            
            var actualTotalTimeProportion1 = actualBucket1 / (actualBucket1+actualBucket2);
            var actualTotalTimeProportion2 = actualBucket2 / (actualBucket1+actualBucket2);

            var propDiff1 = predictedTotalTimeProportion1 - actualTotalTimeProportion1;
            var propDiff2 = predictedTotalTimeProportion2 - actualTotalTimeProportion2;
            
            Console.WriteLine($"proportion1 diff: {Math.Round(propDiff1*100, 3)}%");
            Console.WriteLine($"proportion2 diff: {Math.Round(propDiff2*100, 3)}%");

            Assert.Less( Math.Abs( propDiff1 ), 0.0185 );
            Assert.Less( Math.Abs( propDiff2 ), 0.0185 );
        }
    }
}