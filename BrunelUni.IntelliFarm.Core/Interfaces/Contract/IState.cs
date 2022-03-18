using System.Collections.Generic;
using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IState
    {
        public List<SceneDto> Scenes { get; set; }
        public List<BucketDto> Buckets { get; set; }
        public List<ClientDto> Clients { get; set; }
        public List<FrameDto> Frames { get; set; }
    }
}