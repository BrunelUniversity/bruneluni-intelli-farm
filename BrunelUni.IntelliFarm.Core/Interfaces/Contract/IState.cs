using System.Collections.Generic;
using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;
using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    [ Service( Scope = ServiceLifetimeEnum.Singleton ) ]
    public interface IState
    {
        public List<SceneDto> Scenes { get; set; }
        public List<BucketDto> Buckets { get; set; }
        public List<ClientDto> Clients { get; set; }
        public List<FrameDto> Frames { get; set; }
    }
}