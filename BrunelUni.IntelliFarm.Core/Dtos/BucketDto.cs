using System;
using System.Collections.Generic;
using BrunelUni.IntelliFarm.Core.Enums;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class BucketDto
    {
        public Guid Id { get; set; } = Guid.NewGuid( );
        public Guid SceneId { get; set; }
        public Guid DeviceId { get; set; }
        public List<FrameTimeDto> Frames { get; set; }
        public BucketTypeEnum Type { get; set; }
    }
}