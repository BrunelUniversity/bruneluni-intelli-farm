namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class FrameMetaData
    {
        public int Samples { get; set; }
        public int MaxDiffuseBounces { get; set; }
        public int TriangleCount { get; set; }
        public double ViewportCoverage { get; set; }
        public double SceneCoverage { get; set; }
    }
}