using System;
using Aidan.Common.Core;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class BatchService : IBatchService
    {
        private readonly IConfigurationAdapter _configurationAdapter;

        public BatchService( IConfigurationAdapter configurationAdapter )
        {
            _configurationAdapter = configurationAdapter;
        }

        public Guid Session { get; set; }

        public Result Initialize( )
        {
            var batchModeOptions = _configurationAdapter.Get<TestAppOptions>( ).BatchModeOptions;
            var currentPoly = batchModeOptions.StartPoly;
            var fileCount = 0;
            for( var i = 0; i < batchModeOptions.PolySteps; i++ )
            {
                var currentCoverage = batchModeOptions.StartCoverage;
                for( var j = 0; j < batchModeOptions.CoverageSteps; j++ )
                {
                    var filename = $"poly_{currentPoly}_{currentCoverage.ToString( ).Replace( ".", "_" )}_coverage";
                    Environment.SetEnvironmentVariable( $"Files__{fileCount}__File",
                        $"{batchModeOptions.BasePath}\\{filename}.blend" );
                    Environment.SetEnvironmentVariable( $"Files__{fileCount}__Id", $"{filename}" );
                    Environment.SetEnvironmentVariable( $"Files__{fileCount}__PolyCount", $"{currentPoly}" );
                    Environment.SetEnvironmentVariable( $"Files__{fileCount}__Coverage", $"{currentCoverage}" );
                    currentCoverage -= batchModeOptions.CoverageIncrement;
                    fileCount++;
                }

                currentPoly *= batchModeOptions.PolyMultiplier;
            }

            return Result.Success( );
        }
    }
}