using Aidan.Common.Core.Enum;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.Data.DIModule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrunelUni.IntelliFarm.API
{
    public class Program
    {
        public static void Main( string [ ] args ) { CreateHostBuilder( args ).Build( ).Run( ); }

        public static IHostBuilder CreateHostBuilder( string [ ] args ) =>
            Host.CreateDefaultBuilder( args )
                .ConfigureWebHostDefaults( webBuilder => { webBuilder.UseStartup<Startup>( ); } );
    }

    public class Startup
    {
        public void ConfigureServices( IServiceCollection serviceCollection ) =>
            serviceCollection
                .BindCrosscuttingLayer( )
                .BindDataLayer( )
                .AddTransient<MvcAdapter>( )
                .AddControllers( )
                .BindJsonOptions( CaseEnum.Snake );

        public void Configure( IApplicationBuilder applicationBuilder, IWebHostEnvironment env )
        {
            applicationBuilder
                .UseRouting( )
                .UseEndpoints( endpoints => endpoints.MapControllers( ) );
        }
    }
}