using System;

namespace BrunelUni.IntelliFarm.Data.Core.Dtos
{
    public class FeasabilityDto
    {
        public Guid Id { get; } = Guid.NewGuid( );
        public float PolyCount { get; set; }
        public float Coverage { get; set; }
        public int Samples { get; set; }
        public int MaxBounces { get; set; }
        public double RenderTimeSeconds { get; set; }
        public Guid Session { get; set; }
        public string Device { get; set; }
    }
}