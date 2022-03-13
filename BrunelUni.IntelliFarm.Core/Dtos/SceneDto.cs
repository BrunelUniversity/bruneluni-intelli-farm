using System;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class SceneDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public FrameDto[] Frames { get; set; }
    }
}