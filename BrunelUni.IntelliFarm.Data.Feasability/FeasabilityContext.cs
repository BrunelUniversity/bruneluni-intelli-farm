using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public class FeasabilityContext : DbContext
    {
        private readonly string _fileName;

        public FeasabilityContext( IConfigurationAdapter configurationAdapter )
        {
            _fileName = configurationAdapter.Get<TestAppOptions>( ).DbSource;
        }

        public DbSet<FeasabilityDto> FeasabilityDtos { get; set; }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            optionsBuilder.UseSqlite( $"Data Source={_fileName};" );
        }
    }
}