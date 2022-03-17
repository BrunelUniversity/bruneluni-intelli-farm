using Aidan.Common.Core.Enum;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.Data.DIModule;
using BrunelUni.IntelliFarm.DIModule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrunelUni.IntelliFarm.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup( IConfiguration configuration ) { _configuration = configuration; }

        public void ConfigureServices( IServiceCollection serviceCollection )
        {
            serviceCollection
                .BindCrosscuttingLayer( )
                .BindDataLayer( )
                .BindIntelliFarm( )
                .AddTransient<MvcAdapter>( );
            serviceCollection.AddControllers( )
                .BindJsonOptions( CaseEnum.Snake );
        }

        public void Configure( IApplicationBuilder applicationBuilder, IWebHostEnvironment env ) =>
            applicationBuilder
                .UseRouting( )
                .UseEndpoints( endpoints => endpoints.MapControllers( ) );
    }
}