using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    [ TestFixtureSource( nameof( WeyOrderFixture2 ) ) ]
    public class When_Load_Is_Balanced_Across_4_Unequal_Nodes : Given_A_RenderAnalyser
    {
        private readonly List<PredictorFixtureDto> _predictorFixtureDtos;
        private List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)> _results = new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)>();
        private List<( double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)> _orderedResults;

        public When_Load_Is_Balanced_Across_4_Unequal_Nodes( List<PredictorFixtureDto> predictorFixtureDtos )
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
            
            // 3rd slowest == calculations are relative to speed of bucket 1 (fastest completing bucket)
            var bucket2Speed = 0.5;
            
            // 2nd slowest == calculations are relative to speed of bucket 1 (fastest completing bucket)
            var bucket3Speed = 0.35;
            
            // slowest == calculations are relative to speed of bucket 1 (fastest completing bucket)
            var bucket4Speed = 0.25;
            
            var totalBucketSpeedDivisor = bucket1Speed + bucket2Speed + bucket3Speed + bucket4Speed;

            var bucket1Mult = bucket1Speed / totalBucketSpeedDivisor;
            var bucket2Mult = bucket2Speed / totalBucketSpeedDivisor;
            var bucket3Mult = bucket3Speed / totalBucketSpeedDivisor;
            var bucket4Mult = bucket4Speed / totalBucketSpeedDivisor;

            var totalTime = _orderedResults.Sum( x => x.predictedRenderTime );

            Console.WriteLine($"total time: {totalTime}");
            
            var time1Bucket = totalTime * bucket1Mult;
            var time2Bucket = totalTime * bucket2Mult;
            var time3Bucket = totalTime * bucket3Mult;
            var time4Bucket = totalTime * bucket4Mult;
            
            Console.WriteLine($"total time 1: {time1Bucket}");
            Console.WriteLine($"total time 2: {time2Bucket}");
            Console.WriteLine($"total time 3: {time3Bucket}");
            Console.WriteLine($"total time 4: {time4Bucket}");
            
            var bucket1 =
                new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef
                    )>( );
            var bucket2 =
                new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef
                    )>( );
            var bucket3 =
                new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef
                    )>( );
            var bucket4 =
                new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef
                    )>( );
            var count = 0;
            var bucket1Complete = false;
            var bucket2Complete = false;
            var bucket3Complete = false;
            var bucket4Complete = false;
            var totalOfWhole = _orderedResults.Count;
            var smallestValue = _orderedResults.Min( x => x.predictedRenderTime );
            while(_orderedResults.Any())
            {
                var result = _orderedResults[ 0 ];

                var bucket1Times = bucket1.Sum( x => x.predictedRenderTime );
                var bucket2Times = bucket2.Sum( x => x.predictedRenderTime );
                var bucket3Times = bucket3.Sum( x => x.predictedRenderTime );
                var bucket4Times = bucket4.Sum( x => x.predictedRenderTime );
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
                
                if( _orderedResults.Any( ) )
                {
                    result = _orderedResults[ 0 ];
                }
                else
                {
                    break;
                }

                if( !bucket3Complete )
                {
                    if( bucket3Times + result.predictedRenderTime > time3Bucket )
                    {
                        foreach( var newResult in _orderedResults )
                        {
                            if( bucket3Times + newResult.predictedRenderTime > time3Bucket )
                            {
                                if( _orderedResults.IndexOf( newResult ) == _orderedResults.Count - 1 )
                                {
                                    bucket3.Add( newResult );
                                    _orderedResults.Remove( newResult );
                                    bucket3Complete = true;
                                    break;
                                }
                            }
                            else
                            {
                                bucket3.Add( newResult );
                                _orderedResults.Remove( newResult );
                                break;
                            }
                        }
                    }
                    else
                    {
                        bucket3.Add( result );
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

                if( !bucket4Complete )
                {
                    if( bucket4Times + result.predictedRenderTime > time4Bucket )
                    {
                        foreach( var newResult in _orderedResults )
                        {
                            if( bucket4Times + newResult.predictedRenderTime > time4Bucket )
                            {
                                if( _orderedResults.IndexOf( newResult ) == _orderedResults.Count - 1 )
                                {
                                    bucket4.Add( newResult );
                                    _orderedResults.Remove( newResult );
                                    bucket4Complete = true;
                                    break;
                                }
                            }
                            else
                            {
                                bucket4.Add( newResult );
                                _orderedResults.Remove( newResult );
                                break;
                            }
                        }
                    }
                    else
                    {
                        bucket4.Add( result );
                        _orderedResults.Remove( result );
                    }
                }
            }

            Console.WriteLine($"total between buckets: {bucket1.Count + bucket2.Count + bucket3.Count + bucket4.Count}, total of whole list: {totalOfWhole}");
            
            var predictedBucket1 = bucket1.Sum( x => x.predictedRenderTime );
            var predictedBucket2 = bucket2.Sum( x => x.predictedRenderTime );
            var predictedBucket3 = bucket3.Sum( x => x.predictedRenderTime );
            var predictedBucket4 = bucket4.Sum( x => x.predictedRenderTime );
            
            var actualBucket1 = bucket1.Sum( x => x.actualRenderTime );
            var actualBucket2 = bucket2.Sum( x => x.actualRenderTime );
            var actualBucket3 = bucket3.Sum( x => x.actualRenderTime );
            var actualBucket4 = bucket4.Sum( x => x.actualRenderTime );

            var predictedTotalTimeProportion1 = predictedBucket1 / (predictedBucket1+predictedBucket2+predictedBucket3+predictedBucket4);
            var predictedTotalTimeProportion2 = predictedBucket2 / (predictedBucket1+predictedBucket2+predictedBucket3+predictedBucket4);
            var predictedTotalTimeProportion3 = predictedBucket3 / (predictedBucket1+predictedBucket2+predictedBucket3+predictedBucket4);
            var predictedTotalTimeProportion4 = predictedBucket4 / (predictedBucket1+predictedBucket2+predictedBucket3+predictedBucket4);
            
            var actualTotalTimeProportion1 = actualBucket1 / (actualBucket1+actualBucket2+actualBucket3+actualBucket4);
            var actualTotalTimeProportion2 = actualBucket2 / (actualBucket1+actualBucket2+actualBucket3+actualBucket4);
            var actualTotalTimeProportion3 = actualBucket3 / (actualBucket1+actualBucket2+actualBucket3+actualBucket4);
            var actualTotalTimeProportion4 = actualBucket4 / (actualBucket1+actualBucket2+actualBucket3+actualBucket4);

            var propDiff1 = predictedTotalTimeProportion1 - actualTotalTimeProportion1;
            var propDiff2 = predictedTotalTimeProportion2 - actualTotalTimeProportion2;
            var propDiff3 = predictedTotalTimeProportion3 - actualTotalTimeProportion3;
            var propDiff4 = predictedTotalTimeProportion4 - actualTotalTimeProportion4;
            
            Console.WriteLine($"proportion1 diff: {Math.Round(propDiff1*100, 3)}%");
            Console.WriteLine($"proportion2 diff: {Math.Round(propDiff2*100, 3)}%");
            Console.WriteLine($"proportion3 diff: {Math.Round(propDiff3*100, 3)}%");
            Console.WriteLine($"proportion4 diff: {Math.Round(propDiff4*100, 3)}%");

            Assert.Less( Math.Abs( propDiff1 ), 0.0185 );
            Assert.Less( Math.Abs( propDiff2 ), 0.0185 );
            Assert.Less( Math.Abs( propDiff3 ), 0.0185 );
            Assert.Less( Math.Abs( propDiff4 ), 0.0185 );
        }
    }
}