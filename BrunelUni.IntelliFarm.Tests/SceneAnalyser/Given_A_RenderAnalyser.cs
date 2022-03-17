using System;
using System.Collections.Generic;
using System.Linq;
using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Domain;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.SceneAnalyser
{
    public class Given_A_RenderAnalyser : GivenWhenThen<IRenderAnalyser>
    {
        protected override void Given( )
        {
            SUT = new BlenderCyclesRenderAnalyser( );
        }

        public static PredictorFixtureDto [ ] WeyFixture => FixtureHelper.WeyFixture;
        public static object[ ] WeyOrderFixture1 => FixtureHelper.GetWeyOrderFixture1;
        public static object[ ] WeyOrderFixture2 => FixtureHelper.GetWeyOrderFixture2;
        public void AssertToleranceForBucket( List<PredictorFixtureDto> predictorFixtureDtos,
            List<BucketDto> buckets,
            int index,
            double tolerance,
            double mult )
        {
            var bucket = buckets[ index ];
            var predictedBucket = bucket.Frames.Sum( x => x.Time );
            var actualBucket = new List<double>( );
            foreach( var frame in bucket.Frames )
            {
                actualBucket.Add( predictorFixtureDtos.First( x => x.Frame.Id == frame.Id ).ActualRenderTime );
            }
            var actualBucketTimes = actualBucket.Sum( );
            Console.WriteLine($"actual time for node{index}: {mult*actualBucketTimes}");
            System.Diagnostics.Debug.WriteLine($"predicted time for node{index}: {mult*predictedBucket}");
            var totalPredictedTime = buckets.SelectMany( x => x.Frames ).Sum( x => x.Time );
            var totalActualTime = predictorFixtureDtos.Sum( x => x.ActualRenderTime );
            var predictedTotalTimeProportion = predictedBucket / ( totalPredictedTime );
            var actualTotalTimeProportion = actualBucketTimes / ( totalActualTime );
            var propDiff = predictedTotalTimeProportion - actualTotalTimeProportion;
            Console.WriteLine( $"proportion{index} diff: {Math.Round( propDiff * 100, 3 )}%" );
            Assert.Less( Math.Abs( propDiff ), tolerance );
        }

        public List<BucketDto> LoadBalance( List<PredictorFixtureDto> predictorFixtureDtos, double[] speedsRelativeToFirst )
        {
            var fastClient = FixtureHelper.GetWeyClientData;
            var slowerClients = new [ ] { fastClient }.Concat( speedsRelativeToFirst.Select( x => new ClientDto
            {
                // wey test dataset has no 0 poly viewpoint count data!
                TimeFor0PolyViewpoint = 5,
                TimeFor80Poly100Coverage0Bounces100Samples =
                    x * fastClient.TimeFor80Poly100Coverage0Bounces100Samples
            } ) );
            return SUT.GetBuckets( slowerClients.ToArray( ), predictorFixtureDtos.Select( x => x.Frame ).ToArray( ) )
                .ToList( );
        }
    }
}