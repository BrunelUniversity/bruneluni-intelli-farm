using System.Collections.Generic;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class TestAppOptions
    {
        public bool Local { get; set; }
        public IEnumerable<FileDto> Files { get; set; }
        public SamplesTestAppOptions SamplesTestAppOptions { get; set; }
        public BouncesTestAppOptions BouncesTestAppOptions { get; set; }
    }

    public class FileDto
    {
        public string File { get; set; }
        public string Id { get; set; }
    }
}