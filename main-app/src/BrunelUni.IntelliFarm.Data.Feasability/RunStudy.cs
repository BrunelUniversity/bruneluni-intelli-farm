using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Core.Interfaces.Excluded;
using Aidan.Common.Utils.Utils;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.DIModule;
using BrunelUni.IntelliFarm.Data.Feasability.SamplesTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public abstract class RunStudy<T> where T : IRunnable
    {
        private IHost _host;

        public void SetupAndRun( string [ ] args )
        {
            Bootstrap( );
            _host.Services
                .GetService<ICommandLineArgs>( )!
                .Args = args;
            foreach( var initialisable in _host.Services.GetServices<IInitialisable>( ) ) initialisable.Initialize( );
            Run( );
        }

        private void Run( )
        {
            ActivatorUtilities
                .CreateInstance<T>( _host.Services )
                .Run( );
        }

        private void Bootstrap( ) =>
            _host = Host.CreateDefaultBuilder( )
                .ConfigureServices( ( hostContext, services ) =>
                    services.BindCrosscuttingLayer( )
                        .BindDataLayer( )
                        .AddTransient<SamplesTestApp>( )
                        .AddSingleton<SamplesState>( )
                        .AddTransient<ISerializer, JsonSnakeCaseSerialzier>( )
                        .AddTransient<FeasabilityContext>( ) )
                .Build( );
    }
}