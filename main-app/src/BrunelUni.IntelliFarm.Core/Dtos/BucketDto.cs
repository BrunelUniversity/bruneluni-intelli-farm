using System;
using System.Collections.Generic;
using System.ComponentModel;
using BrunelUni.IntelliFarm.Core.Enums;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class BucketDto
    {
        public Guid Id { get; set; } = Guid.NewGuid( );
        public Guid SceneId { get; set; }
        [ Description( "file key in s3" ) ] public string FilePath { get; set; }
        public Guid DeviceId { get; set; }
        public List<FrameTimeDto> Frames { get; set; }
        public BucketTypeEnum Type { get; set; }
    }
}