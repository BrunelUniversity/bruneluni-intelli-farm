using System;
using System.Collections.Generic;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class BucketDto
    {
        public Guid Id { get; set; } = Guid.NewGuid( );
        public Guid DeviceId { get; set; }
        public List<(double predictedTime, Guid id)> Frames { get; set; }
    }
}