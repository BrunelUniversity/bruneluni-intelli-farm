using System;
using System.ComponentModel;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class JobDto
    {
        public Guid SceneId { get; set; }
        public Guid DeviceId { get; set; }
        [ Description("numbers of frames that will be rendered in job") ]
        public int[] FrameNumbers { get; set; }
    }
}