using Aidan.Common.Core.Enum;
using Aidan.Common.Utils.Web;
using Auth0.AspNetCore.Authentication;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.Data.DIModule;
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
                .AddTransient<MvcAdapter>( )
                .AddAuth0WebAppAuthentication( options =>
                {
                    var appOptions = _configuration.Get<AppOptions>( );
                    options.Domain = appOptions.Auth0Domain;
                    options.ClientId = appOptions.Auth0ClientId;
                } );
            serviceCollection.AddControllers( )
                .BindJsonOptions( CaseEnum.Snake );
        }

        public void Configure( IApplicationBuilder applicationBuilder, IWebHostEnvironment env ) =>
            applicationBuilder
                .UseAuthentication( )
                .UseAuthorization( )
                .UseRouting( )
                .UseEndpoints( endpoints => endpoints.MapControllers( ) );
    }
}