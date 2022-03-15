using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    [ TestFixtureSource( nameof( WeyOrderFixture ) ) ]
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
                _results.Add( ( dto.ActualRenderTime, SUT.GetPredictedTime( FixtureHelper.GetWeyCalibrationData, dto.FrameMetaData ), dto ) );
            }

            _orderedResults = _results.OrderBy( x => x.predictedRenderTime ).ToList( );

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
            foreach( var result in _orderedResults )
            {
                try
                {
                    if( count == 1 )
                    {
                        count = 0;
                        var nextIndex = _orderedResults.IndexOf( result ) + 1;
                        bucket1.Add( result );
                        bucket2.Add( _orderedResults[ nextIndex ] );
                    }
                }
                catch( Exception e ) when( e is ArgumentOutOfRangeException || e is IndexOutOfRangeException )
                {
                    
                }
                count++;
            }

            var sumBucket1 = bucket1.Sum( x => x.actualRenderTime );
            var sumBucket2 = bucket2.Sum( x => x.actualRenderTime );
            Assert.Less( Math.Abs( sumBucket1 - sumBucket2 ), sumBucket1 * 0.0085 );
        }
    }
}