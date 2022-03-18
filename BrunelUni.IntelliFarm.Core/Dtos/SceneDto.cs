using System;
using System.ComponentModel;
using BrunelUni.IntelliFarm.Core.Enums;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class SceneDto
    {
        public Guid Id { get; set; }

        [ Description( "key of file in s3 bucket" ) ]
        public string FileName { get; set; }

        public int StartFrame { get; set; }
        public FrameDto [ ] Frames { get; set; }
        public RenderStatusEnum Status { get; set; } = RenderStatusEnum.NotStarted;
    }
}