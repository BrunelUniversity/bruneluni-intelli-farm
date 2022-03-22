using System;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class FrameDto
    {
        public Guid Id { get; set; } = Guid.NewGuid( );
        public Guid Scene { get; set; }
        public int Number { get; set; }
        public int Samples { get; set; }
        public int MaxDiffuseBounces { get; set; }
        public int TriangleCount { get; set; }
        public double ViewportCoverage { get; set; }
        public double SceneCoverage { get; set; }
    }
}