using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    [ TestFixtureSource( nameof( WeyOrderFixture2 ) ) ]
    public class When_Load_Is_Balanced_Across_2_Equal_Nodes : Given_A_RenderAnalyser
    {
        private readonly List<PredictorFixtureDto> _predictorFixtureDtos;
        private List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)> _results = new List<(double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)>();
        private List<( double predictedRenderTime, double actualRenderTime, PredictorFixtureDto predictorDataRef)> _orderedResults;

        public When_Load_Is_Balanced_Across_2_Equal_Nodes( List<PredictorFixtureDto> predictorFixtureDtos )
        {
            _predictorFixtureDtos = predictorFixtureDtos;
        }

        protected override void When( )
        {
            foreach( var dto in _predictorFixtureDtos )
            {
                _results.Add( ( SUT.GetPredictedTime( FixtureHelper.GetWeyClientData, dto.Frame ), dto.ActualRenderTime, dto ) );
            }

            _orderedResults = _results.OrderByDescending( x => x.predictedRenderTime ).ToList( );

        }

        [ Test ]
        public void Then_The_Difference_In_Render_Time_Must_Be_Within_A_Given_Tolerance( )
        {
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
            var totalTimeForEach = _orderedResults.Sum( x => x.predictedRenderTime ) / 2;
            while(_orderedResults.Any())
            {
                var result = _orderedResults[ 0 ];

                var bucket1Times = bucket1.Sum( x => x.predictedRenderTime );
                var bucket2Times = bucket2.Sum( x => x.predictedRenderTime );
                if( !bucket1Complete )
                {
                    if( bucket1Times + result.predictedRenderTime > totalTimeForEach )
                    {
                        foreach( var newResult in _orderedResults )
                        {
                            if( bucket1Times + newResult.predictedRenderTime > totalTimeForEach )
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

                result = _orderedResults[ 0 ];
                if( !bucket2Complete )
                {
                    if( bucket2Times + result.predictedRenderTime > totalTimeForEach )
                    {
                        foreach( var newResult in _orderedResults )
                        {
                            if( bucket2Times + newResult.predictedRenderTime > totalTimeForEach )
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
            
            Console.WriteLine( $"predicted bucket 1: {bucket1.Sum( x => x.predictedRenderTime )}" );
            Console.WriteLine( $"predicted bucket 2: {bucket2.Sum( x => x.predictedRenderTime )}" );
            
            Console.WriteLine( $"actual bucket 1: {bucket1.Sum( x => x.actualRenderTime )}" );
            Console.WriteLine( $"actual bucket 2: {bucket2.Sum( x => x.actualRenderTime )}" );
            
            var sumBucket1 = bucket1.Sum( x => x.actualRenderTime );
            var sumBucket2 = bucket2.Sum( x => x.actualRenderTime );
            
            Console.WriteLine( $"load imbalance seconds: {Math.Abs( sumBucket1 - sumBucket2 )}" );
            
            Console.WriteLine($"{Math.Round(Math.Abs(((sumBucket1 - sumBucket2)/(totalTimeForEach * 2)))*100, 2)}% imbalance");
            
            Assert.Less( Math.Abs( sumBucket1 - sumBucket2 ), ( totalTimeForEach * 2 * 0.02 ) + smallestValue );
        }
    }
}