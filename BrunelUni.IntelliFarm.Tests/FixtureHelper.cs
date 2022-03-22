using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.DataAccess;
using Newtonsoft.Json;

namespace BrunelUni.IntelliFarm.Tests
{
    public class FixtureHelper
    {
        private static List<object [ ]> Data = new List<object [ ]>( );
        
        public static void AddToReport( object[] data )
        {
            Data.Add( data );
            using var fileStream = File.OpenWrite( $"{Directory.GetCurrentDirectory( )}\\report.json" );
            fileStream.Write( Encoding.UTF8.GetBytes( JsonConvert.SerializeObject( Data, Formatting.Indented ) ) );
        }

        [ System.ComponentModel.Description( "loads fixture from wey laptop" ) ]
        private static IEnumerable<FeasabilityDto> LoadWeyFixture( )
        {
            var fileService = new S3RemoteFileService( Environment.GetEnvironmentVariable( "AWS_ID" ),
                Environment.GetEnvironmentVariable( "AWS_TOKEN" ) );
            var contents = fileService.Get( "test-fixture-data/fixture-data-wey.json" );
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
                    Frame = new FrameDto
                    {
                        MaxDiffuseBounces = x.MaxBounces,
                        Samples = x.Samples,
                        SceneCoverage = x.Coverage,
                        TriangleCount = ( int )x.PolyCount,
                        ViewportCoverage = 100
                    },
                    ActualRenderTime = x.RenderTimeSeconds
                } ).ToList( );
                foreach( var fixture in fixtureData )
                {
                    fixture.Frame.Number = fixtureData.IndexOf( fixture );
                }

                return fixtureData.ToArray( );
            }
        }

        public static ClientDto GetWeyClientData => new ClientDto
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