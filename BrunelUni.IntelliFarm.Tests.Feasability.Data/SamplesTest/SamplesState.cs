using System.Collections.Generic;
using Aidan.Common.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Tests.Feasability.Data.SamplesTest
{
    public class SamplesState
    {
        public SamplesState( IConfigurationAdapter configurationAdapter )
        {
            var state = configurationAdapter.Get<TestAppOptions>( );
            RenderResults = new List<RenderSamplesResultDto>( );
            Iterations = state.SamplesTestAppOptions.Iterations;
            File = state.File;
            Samples = state.SamplesTestAppOptions.Samples;
            SamplesDiff = state.SamplesTestAppOptions.SamplesDiff;
        }
        public IList<RenderSamplesResultDto> RenderResults { get; set; }
        public int Iterations { get; set; }
        public string File { get; set; }
        public int Samples { get; set; }
        public int SamplesDiff { get; set; }
    }
}