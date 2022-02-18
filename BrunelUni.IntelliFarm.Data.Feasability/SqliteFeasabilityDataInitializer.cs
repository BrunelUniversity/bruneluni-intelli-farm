using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class SqliteFeasabilityDataInitializer : IFeasabilityDataInitializer
    {
        private readonly FeasabilityContext _feasabilityContext;

        public SqliteFeasabilityDataInitializer( FeasabilityContext feasabilityContext )
        {
            _feasabilityContext = feasabilityContext;
        }

        public Result Initialize( )
        {
            _feasabilityContext.Database.EnsureCreated( );
            return Result.Success( );
        }
    }
}