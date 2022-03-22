using System.Collections.Generic;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class State : IState
    {
        public List<SceneDto> Scenes { get; set; } = new List<SceneDto>( );
        public List<BucketDto> Buckets { get; set; } = new List<BucketDto>( );
        public List<ClientDto> Clients { get; set; } = new List<ClientDto>( );
        public List<FrameDto> Frames { get; set; } = new List<FrameDto>( );
    }
}