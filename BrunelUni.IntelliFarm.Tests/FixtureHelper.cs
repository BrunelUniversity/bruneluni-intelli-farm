using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using Newtonsoft.Json;

namespace BrunelUni.IntelliFarm.Tests
{
    public class FixtureHelper
    {
        [ System.ComponentModel.Description( "loads fixture from wey laptop" ) ]
        private static IEnumerable<FeasabilityDto> LoadWeyFixture( )
        {
            using var client = new AmazonS3Client(
                Environment.GetEnvironmentVariable( "AWS_ID" ),
                Environment.GetEnvironmentVariable( "AWS_TOKEN" ),
                RegionEndpoint.GetBySystemName( "eu-west-2" ) );
            var request = new GetPreSignedUrlRequest
            {
                BucketName = "intelli-farm",
                Key = "test-fixture-data/fixture-data-wey.json",
                Expires = DateTime.UtcNow.AddMinutes( 10 ),
                Verb = HttpVerb.GET,
                Protocol = Protocol.HTTPS
            };
            var url = client.GetPreSignedURL( request );
            var req = WebRequest.Create( url );
            var resp = req.GetResponse( );

            using var sr = new StreamReader( resp.GetResponseStream( ) );

            var contents = sr.ReadToEnd( );
            var dtos = JsonConvert.DeserializeObject<List<FeasabilityDto>>( contents );
            return dtos;
        }

        [ System.ComponentModel.Description(
            "data fixture from the wey laptop feasability study, used to generate the initial heuristics for the system" ) ]
        public static PredictorFixtureDto [ ] WeyFixture
        {
            get
            {
                var data = LoadWeyFixture( );
                var fixtureData = data.Select( x => new PredictorFixtureDto
                {
                    FrameMetaData = new FrameMetaData
                    {
                        MaxDiffuseBounces = x.MaxBounces,
                        Samples = x.Samples,
                        SceneCoverage = x.Coverage,
                        TriangleCount = ( int )x.PolyCount,
                        ViewportCoverage = 100
                    },
                    ActualRenderTime = x.RenderTimeSeconds
                } ).ToArray( );
                return fixtureData;
            }
        }

        public static CallibrationDto GetWeyCalibrationData => new CallibrationDto
        {
            // time for 0 poly count isn't known for the wey data fixture but viewport coverage is always 100%
            TimeFor0PolyViewpoint = 7,
            TimeFor80Poly100Coverage0Bounces100Samples = 97
        };

        public static object [ ] GetWeyOrderFixture1
        {
            get
            {
                var weyFixture = WeyFixture;
                var toReturn = weyFixture.ToList( )
                    .Select( ( x, i ) => new { Index = i, Value = x } )
                    .GroupBy( x => x.Index / 1337 )
                    .Select( x => new PredictorFixtureDtoCollection( x.Select( v => v.Value ).ToList( ) ) )
                    .ToList( );
                return toReturn.ToArray( );
            }
        }
        
        public static object [ ] GetWeyOrderFixture2
        {
            get
            {
                var weyFixture = WeyFixture;
                var toReturn = weyFixture.ToList( )
                    .Select( ( x, i ) => new { Index = i, Value = x } )
                    .GroupBy( x => x.Index / 100 )
                    .Select( x => new PredictorFixtureDtoCollection( x.Select( v => v.Value ).ToList( ) ) )
                    .ToList( );
                var toReturn500 = weyFixture.ToList( )
                    .Select( ( x, i ) => new { Index = i, Value = x } )
                    .GroupBy( x => x.Index / 500 )
                    .Select( x => new PredictorFixtureDtoCollection( x.Select( v => v.Value ).ToList( ) ) )
                    .ToList( );
                toReturn.AddRange( toReturn500 );
                toReturn.Add( new PredictorFixtureDtoCollection( weyFixture.ToList( ) ) );
                return toReturn.ToArray( );
            }
        }
    }
}