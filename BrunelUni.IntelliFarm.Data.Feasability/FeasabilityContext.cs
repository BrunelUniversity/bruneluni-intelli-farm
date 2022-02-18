using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class FeasabilityContext : DbContext
    {
        private readonly string _source;

        public FeasabilityContext( IConfigurationAdapter configurationAdapter )
        {
            _source = configurationAdapter.Get<TestAppOptions>( ).DbSource;
        }

        public DbSet<FeasabilityDto> FeasabilityDtos { get; set; }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            optionsBuilder.UseMySql( _source );
        }
    }
}