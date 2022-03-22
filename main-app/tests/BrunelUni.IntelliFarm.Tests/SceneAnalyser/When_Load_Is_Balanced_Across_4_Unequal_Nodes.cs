using System.Collections.Generic;
using BrunelUni.IntelliFarm.Core.Dtos;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    [ TestFixtureSource( nameof( WeyOrderFixture2 ) ) ]
    public class When_Load_Is_Balanced_Across_4_Unequal_Nodes : Given_A_RenderAnalyser
    {
        private readonly List<PredictorFixtureDto> _predictorFixtureDtos;
        private List<BucketDto> _results = new List<BucketDto>( );

        public When_Load_Is_Balanced_Across_4_Unequal_Nodes( List<PredictorFixtureDto> predictorFixtureDtos )
        {
            _predictorFixtureDtos = predictorFixtureDtos;
        }

        protected override void When( )
        {
            _results = LoadBalance( _predictorFixtureDtos, new [ ] { 2.0, 2.5, 3 } );
        }

        [ Test ]
        public void Then_The_Difference_In_Render_Time_Must_Be_Within_A_Given_Tolerance( )
        {
            var prop1 = AssertToleranceForBucket( _predictorFixtureDtos, _results, 0, 0.0185, 1.0 );
            var prop2 = AssertToleranceForBucket( _predictorFixtureDtos, _results, 1, 0.0185, 2.0 );
            var prop3 = AssertToleranceForBucket( _predictorFixtureDtos, _results, 2, 0.0185, 2.5 );
            var prop4 = AssertToleranceForBucket( _predictorFixtureDtos, _results, 3, 0.0185, 2.5 );
            FixtureHelper.AddToReport( new [ ] { prop1, prop2, prop3, prop4 } );
        }
    }
}