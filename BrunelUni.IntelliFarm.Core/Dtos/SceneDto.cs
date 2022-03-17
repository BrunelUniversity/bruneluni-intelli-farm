using System;
using System.ComponentModel;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class SceneDto
    {
        public Guid Id { get; set; }

        [ Description( "key of file in s3 bucket" ) ]
        public string FileName { get; set; }

        public FrameDto [ ] Frames { get; set; }
        public string ZipFileBytes { get; set; }
    }
}