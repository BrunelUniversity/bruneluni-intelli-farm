using System;
using System.Collections.Generic;
using System.Linq;
using Aidan.Common.Core;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Feasability.SamplesTest;
using Castle.Core.Internal;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class BatchService : IBatchService
    {
        private readonly IConfigurationAdapter _configurationAdapter;
        private readonly SamplesState _samplesState;

        public BatchService( IConfigurationAdapter configurationAdapter,
            SamplesState samplesState )
        {
            _configurationAdapter = configurationAdapter;
            _samplesState = samplesState;
        }

        public Guid Session { get; set; } = Guid.NewGuid( );

        public Result Initialize( )
        {
            var appOptions = _configurationAdapter.Get<TestAppOptions>( );
            var batchModeOptions = appOptions.BatchModeOptions;
            var currentPoly = batchModeOptions.StartPoly;
            var fileCount = 0;
            if( batchModeOptions.BatchMode )
            {
                _samplesState.Files = new List<FileDto>( );
                for( var i = 0; i < batchModeOptions.PolySteps; i++ )
                {
                    var currentCoverage = batchModeOptions.StartCoverage;
                    for( var j = 0; j < batchModeOptions.CoverageSteps; j++ )
                    {
                        var filename = $"poly_{currentPoly}_{currentCoverage.ToString( ).Replace( ".", "_" )}_coverage";
                        _samplesState.Files.Add( new FileDto
                        {
                            Coverage = currentCoverage,
                            File = $"{batchModeOptions.BasePath}\\{filename}.blend",
                            Id = filename,
                            PolyCount = currentPoly
                        } );
                        if( !appOptions.CurrentFile.IsNullOrEmpty( ) )
                        {
                            var currentFile = _samplesState.Files.First( x => x.Id == appOptions.CurrentFile );
                            _samplesState.Files = _samplesState
                                .Files
                                .Skip( _samplesState.Files.IndexOf( currentFile ) )
                                .ToList( );
                        }

                        currentCoverage -= batchModeOptions.CoverageIncrement;
                        fileCount++;
                    }

                    currentPoly *= batchModeOptions.PolyMultiplier;
                }
            }
            else
            {
                _samplesState.Files = _configurationAdapter
                    .Get<TestAppOptions>( )
                    .Files
                    .ToList( );
            }

            return Result.Success( );
        }
    }
}