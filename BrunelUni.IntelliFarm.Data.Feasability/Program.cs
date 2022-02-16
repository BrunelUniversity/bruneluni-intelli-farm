using System;
using BrunelUni.IntelliFarm.Data.Feasability.SamplesTest;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class Program
    {
        public static void Main( string [ ] args )
        {
            if( args.Length != 0 )
            {
                switch( args[ 0 ] )
                {
                    case "/batch":
                        var polyMultiplier = int.Parse( args[ 1 ] );
                        var coverageIncrement = float.Parse( args[ 2 ] );
                        var polySteps = float.Parse( args[ 3 ] );
                        var coverageSteps = float.Parse( args[ 4 ] );
                        var basePath = args[ 5 ];
                        var currentPoly = 80;
                        var fileCount = 0;
                        for( int i = 0; i < polySteps; i++ )
                        {
                            var currentCoverage = 100.0;
                            for( int j = 0; j < coverageSteps; j++ )
                            {
                                var filename = $"poly_{currentPoly}_coverage_{currentCoverage.ToString().Replace( ".", "_" )}";
                                Environment.SetEnvironmentVariable( $"Files__{fileCount}__File", $"{basePath}\\{filename}.blend" );
                                Environment.SetEnvironmentVariable( $"Files__{fileCount}__Id", $"{filename}" );
                                currentCoverage -= coverageIncrement;
                                fileCount++;
                            }

                            currentPoly *= polyMultiplier;
                        }
                        break;
                }
            }

            new RunSamplesStudy( )
                .SetupAndRun( );
        }
    }
}