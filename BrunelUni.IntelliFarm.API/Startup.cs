using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.Data.DIModule;
using BrunelUni.IntelliFarm.DIModule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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
            serviceCollection.AddControllers( );
            serviceCollection.Configure<KestrelServerOptions>( options =>
            {
                options.AllowSynchronousIO = true;
            } );
        }

        public void Configure( IApplicationBuilder applicationBuilder, IWebHostEnvironment env )
        {
            var state = applicationBuilder.ApplicationServices.GetService<IState>( );
            state.Clients.Add( new ClientDto
            {
                Name = "WEY1",
                TimeFor0PolyViewpoint = 6.7,
                TimeFor80Poly100Coverage0Bounces100Samples = 20.4
            } );
            state.Clients.Add( new ClientDto
            {
                Name = "WEY2",
                TimeFor0PolyViewpoint = 13.4,
                TimeFor80Poly100Coverage0Bounces100Samples = 40.8
            } );
            applicationBuilder
                .UseRouting( )
                .UseEndpoints( endpoints => endpoints.MapControllers( ) );
        }
    }
}