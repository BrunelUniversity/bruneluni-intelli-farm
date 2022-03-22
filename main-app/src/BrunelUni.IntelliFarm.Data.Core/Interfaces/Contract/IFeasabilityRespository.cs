using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface IFeasabilityRespository
    {
        public void Create( FeasabilityDto feasabilityDto );
    }
}