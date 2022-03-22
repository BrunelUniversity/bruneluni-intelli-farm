using System.Collections.Generic;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class TestAppOptions
    {
        public IEnumerable<FileDto> Files { get; set; }
        public SamplesTestAppOptions SamplesTestAppOptions { get; set; }
        public BouncesTestAppOptions BouncesTestAppOptions { get; set; }
        public BatchModeDto BatchModeOptions { get; set; }
        public string DbSource { get; set; }
        public string Device { get; set; }
        public string CurrentFile { get; set; }
    }

    public class FileDto
    {
        public string File { get; set; }
        public string Id { get; set; }
        public float PolyCount { get; set; }
        public float Coverage { get; set; }
    }
}