using System;
using System.Collections.Generic;
using System.Linq;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Domain
{
    public class BlenderCyclesRenderAnalyser : IRenderAnalyser
    {
        public BucketDto [ ] GetBuckets( ClientDto [ ] clients, FrameDto [ ] frames )
        {
            // create buckets
            var buckets = clients.Select( x => new BucketDto
            {
                DeviceId = x.Id,
                Frames = new List<FrameTimeDto>()
            } ).ToList( );

            // node speed calc
            var orderedClients = clients.ToList( ).OrderBy( x => x.TimeFor80Poly100Coverage0Bounces100Samples );
            var firstClient = orderedClients.First( );
            var restOfClients = orderedClients.Skip( 1 );
            var clientsWithRespectToFastestNode = new [ ]
            {
                ( clientDiv: 1.0, client: firstClient.Id )
            }.Concat(
                restOfClients.Select( x => (
                    clientDiv: ( 1 / ( x.TimeFor80Poly100Coverage0Bounces100Samples /
                                       firstClient.TimeFor80Poly100Coverage0Bounces100Samples ) ),
                    client: x.Id ) ) ).ToList( );
            
            // get predicted time for fastest node
            var predictedTimes = frames.Select( x => new FrameTimeDto
                {
                    Id = x.Id,
                    Time = GetPredictedTime( firstClient, x )
                } )
                .ToList( );
            var orderedPredictedTimes = predictedTimes.OrderByDescending( x => x.Time ).ToArray( ).ToList( );
            
            // node multiplier calc
            var totalBucketSpeedDivisor = clientsWithRespectToFastestNode.Sum( x => x.clientDiv );
            var bucketMults = clientsWithRespectToFastestNode.Select( x => (mult: x.clientDiv / totalBucketSpeedDivisor, client: x.client) );

            // total time render time calc
            var totalTime = predictedTimes.Sum( x => x.Time );

            Console.WriteLine($"total time: {totalTime}");

            var totalTimeForNodes = bucketMults.Select( x => (time: totalTime * x.mult, client: x.client) ).ToList( );

            Console.WriteLine( $"total time 1: {totalTimeForNodes[ 0 ]}" );
            Console.WriteLine( $"total time 2: {totalTimeForNodes[ 1 ]}" );

            var bucketFullList = buckets.Select( x => new BucketComplete
            {
                Complete = false,
                DeviceId = x.DeviceId
            } ).ToList(  );
            var totalOfWhole = predictedTimes.Count;
            while( orderedPredictedTimes.Any( ) )
            {
                foreach( var bucket in buckets )
                {
                    AddToBucket( bucket, orderedPredictedTimes,
                        totalTimeForNodes.First( x => x.client == bucket.DeviceId ).time, bucketFullList );
                }
            }

            return buckets.ToArray( );
        }

        private bool AddToBucket( BucketDto bucket,
            List<FrameTimeDto> predictedTimes,
            double maxTimeForBucket,
            List<BucketComplete> bucketCompleted )
        {
            var bucketTimes = bucket.Frames.Sum( x => x.Time );
            FrameTimeDto result;
            if( predictedTimes.Any( ) )
            {
                result = predictedTimes[ 0 ];
            }
            else
            {
                return false;
            }

            if( !bucketCompleted.First(x => x.DeviceId == bucket.DeviceId).Complete )
            {
                if( bucketTimes + result.Time > maxTimeForBucket )
                {
                    foreach( var newResult in predictedTimes )
                    {
                        if( bucketTimes + newResult.Time > maxTimeForBucket )
                        {
                            if( predictedTimes.IndexOf( newResult ) == predictedTimes.Count - 1 )
                            {
                                bucket.Frames.Add( newResult );
                                predictedTimes.Remove( newResult );
                                bucketCompleted.First( x => x.DeviceId == bucket.DeviceId ).Complete = true;
                                break;
                            }
                        }
                        else
                        {
                            bucket.Frames.Add( newResult );
                            predictedTimes.Remove( newResult );
                            break;
                        }
                    }
                }
                else
                {
                    bucket.Frames.Add( result );
                    predictedTimes.Remove( result );
                }
            }

            return true;
        }

        public double GetPredictedTime( ClientDto client, FrameDto frame )
        {
            // poly calc
            var exactPolyRenderTimeFor100Sam100SceneCov100ViewCov0Bounces =
                client.TimeFor80Poly100Coverage0Bounces100Samples;
            if( frame.TriangleCount > 1280 )
            {
                var polyExpMultiplier =
                    client.TimeFor80Poly100Coverage0Bounces100Samples / Math.Pow( 80, SceneHeuristicsConstants.PolyExp );
                exactPolyRenderTimeFor100Sam100SceneCov100ViewCov0Bounces =
                    polyExpMultiplier * Math.Pow( frame.TriangleCount, SceneHeuristicsConstants.PolyExp );
            }

            // samples calc
            var samplesMultiplier = (double) frame.Samples / 100;
            var exactPolyAndSamRenderTimeFor100SceneCov100ViewCov0Bounces =
                samplesMultiplier * exactPolyRenderTimeFor100Sam100SceneCov100ViewCov0Bounces;
            
            // scene coverage calc
            var bounceHmaxFor100Cov = exactPolyAndSamRenderTimeFor100SceneCov100ViewCov0Bounces * SceneHeuristicsConstants.BouncesHmaxLogisticRegressionFunctionMult;
            var bounceHMinExpMult = exactPolyAndSamRenderTimeFor100SceneCov100ViewCov0Bounces /
                              Math.Pow( 8, SceneHeuristicsConstants.BouncesAndCovHMinCalcExp );
            var bounceHMinExactCov = bounceHMinExpMult *
                                     Math.Pow( frame.SceneCoverage * SceneHeuristicsConstants.SceneCoverageMultForHMinExpCalc,
                                         SceneHeuristicsConstants.BouncesAndCovHMinCalcExp );
            var bounceIndex = 8 - ( frame.SceneCoverage * SceneHeuristicsConstants.SceneCoverageMultForHMinExpCalc );
            var bounceHMaxExactCov = bounceHmaxFor100Cov *
                                     Math.Pow( 9 - ( frame.SceneCoverage * SceneHeuristicsConstants.SceneCoverageMultForHMinExpCalc ),
                                         SceneHeuristicsConstants.SceneCoverageMultForHMaxExpCalc );
            var baseBounceRate = SceneHeuristicsConstants.Cov100BounceRate;

            var bounceRate = baseBounceRate + ( bounceIndex * SceneHeuristicsConstants.BounceRateDiff );

            // bounces calc
            var lastValue = bounceHMinExactCov;
            for( var i = 0; i < frame.MaxDiffuseBounces; i++ )
            {
                lastValue = lastValue * ( 1.0 + ( bounceRate * ( 1.0 - ( lastValue / bounceHMaxExactCov ) ) ) );
            }

            var viewportWorldSpace = 100 - frame.ViewportCoverage;

            var viewportWorldTime = client.TimeFor0PolyViewpoint * ( viewportWorldSpace / 100 );
            var viewportObjectsTime = lastValue * ( frame.ViewportCoverage / 100 );

            return viewportWorldTime + viewportObjectsTime;
        }
    }

    public class BucketComplete
    {
        public bool Complete { get; set; }
        public Guid DeviceId { get; set; }
    }
}