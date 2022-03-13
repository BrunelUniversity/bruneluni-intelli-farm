using Aidan.Common.Core.Enum;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.Data.API.Filters;
using BrunelUni.IntelliFarm.Data.DIModule;
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
                .BindDataLayer( )
                .AddTransient<MvcAdapter>( )
                .AddTransient<RenderFileFilter>( )
                .AddTransient<BlenderInstallFilter>( )
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