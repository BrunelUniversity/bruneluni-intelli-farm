using System;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class ClientDto
    {
        public Guid Id { get; set; } = Guid.NewGuid( );
        public string Name { get; set; }
        public double TimeFor0PolyViewpoint { get; set; } = double.NaN;
        public double TimeFor80Poly100Coverage0Bounces100Samples { get; set; } = double.NaN;
    }
}