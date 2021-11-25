﻿using System.Collections.Generic;
using Aidan.Common.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Tests.Feasability.Data.SamplesTest
{
    public class SamplesState
    {
        public SamplesState( IConfigurationAdapter configurationAdapter )
        {
            var state = configurationAdapter.Get<TestAppOptions>( );
            RenderResults = new List<RenderSamplesResultDto>( );
            Files = state.Files;

            Samples = new MetaTestDto( );
            Bounces = new MetaTestDto( );
            Samples.Iterations = state.SamplesTestAppOptions.Iterations;
            Samples.Value = state.SamplesTestAppOptions.Samples;
            Samples.Diff = state.SamplesTestAppOptions.SamplesDiff;
            
            Bounces.Iterations = state.BouncesTestAppOptions.Iterations;
            Bounces.Value = state.BouncesTestAppOptions.Bounces;
            Bounces.Diff = state.BouncesTestAppOptions.BouncesDiff;
        }
        public IList<RenderSamplesResultDto> RenderResults { get; set; }
        public MetaTestDto Samples { get; set; }
        public MetaTestDto Bounces { get; set; }
        public IEnumerable<FileDto> Files { get; set; }
    }
}