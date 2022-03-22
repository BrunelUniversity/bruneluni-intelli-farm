using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class SqliteFeasabilityRespository : IFeasabilityRespository
    {
        private readonly FeasabilityContext _feasabilityContext;

        public SqliteFeasabilityRespository( FeasabilityContext feasabilityContext )
        {
            _feasabilityContext = feasabilityContext;
        }

        public void Create( FeasabilityDto feasabilityDto )
        {
            _feasabilityContext
                .FeasabilityDtos
                .Add( feasabilityDto );
            _feasabilityContext.SaveChanges( );
        }
    }
}