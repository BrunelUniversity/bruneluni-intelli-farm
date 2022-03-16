using System.Collections.Generic;
using BrunelUni.IntelliFarm.Core.Dtos;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    [ TestFixtureSource( nameof( WeyOrderFixture2 ) ) ]
    public class When_Load_Is_Balanced_Across_2_Unequal_Nodes : Given_A_RenderAnalyser
    {
        private readonly List<PredictorFixtureDto> _predictorFixtureDtos;
        private List<BucketDto> _results = new List<BucketDto>( );

        public When_Load_Is_Balanced_Across_2_Unequal_Nodes( List<PredictorFixtureDto> predictorFixtureDtos )
        {
            _predictorFixtureDtos = predictorFixtureDtos;
        }

        protected override void When( )
        {
            _results = LoadBalance( _predictorFixtureDtos, new [ ] { 2.0 } );
        }

        [ Test ]
        public void Then_The_Difference_In_Render_Time_Must_Be_Within_A_Given_Tolerance( )
        {
            AssertToleranceForBucket( _predictorFixtureDtos, _results, 0, 0.0185, 1.0 );
            AssertToleranceForBucket( _predictorFixtureDtos, _results, 1, 0.0185, 2.0 );
        }
    }
}