using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BrunelUni.IntelliFarm.Data.API
{
    public class Startup
    {
        public void ConfigureServices( IServiceCollection serviceCollection ) =>
            serviceCollection
                .BindCrosscuttingLayer( )
                .AddTransient<MvcAdapter>( )
                .AddControllers( );

        public void Configure( IApplicationBuilder applicationBuilder, IWebHostEnvironment env ) =>
            applicationBuilder
                .UseRouting( )
                .UseEndpoints( endpoints => endpoints.MapControllers( ) );
    }
}