using System.Collections.Generic;
using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.API
{
    public class State
    {
        public List<SceneDto> Scenes { get; set; } = new List<SceneDto>( );
        public List<BucketDto> Buckets { get; set; } = new List<BucketDto>( );
        public List<ClientDto> Clients { get; set; } = new List<ClientDto>( );
        public List<FrameDto> Frames { get; set; } = new List<FrameDto>( );
    }
}