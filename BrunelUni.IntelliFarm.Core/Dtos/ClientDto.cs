using System;
using BrunelUni.IntelliFarm.Core.Enums;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class ClientDto
    {
        public Guid Id { get; set; } = Guid.NewGuid( );
        public string Name { get; set; }
        public double TimeFor0PolyViewpoint { get; set; } = 0.0;
        public double TimeFor80Poly100Coverage0Bounces100Samples { get; set; } = 0.0;
        public RenderStatusEnum Progress { get; set; }
    }
}