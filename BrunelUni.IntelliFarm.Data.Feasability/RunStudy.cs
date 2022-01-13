using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Utils;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.Data.DIModule;
using BrunelUni.IntelliFarm.Data.Feasability.SamplesTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BrunelUni.IntelliFarm.Data.Feasability
{
    public abstract class RunStudy<T> where T : IRunnable
    {
        private IHost _host;
        
        public void SetupAndRun( )
        {
            Bootstrap( );
            CustomSetup( _host );
            Run( );
        }

        protected virtual void CustomSetup( IHost host )
        {
        }

        private void Bootstrap( ) =>
            _host = Host.CreateDefaultBuilder( )
                .ConfigureServices( ( hostContext, services ) =>
                    services.BindCrosscuttingLayer( )
                        .BindDataLayer( )
                        .AddTransient<SamplesTestApp>( )
                        .AddSingleton<SamplesState>( )
                        .AddTransient<ISerializer, JsonSnakeCaseSerialzier>( ) )
                .Build( );

        private void Run( )
        {
            ActivatorUtilities
                .CreateInstance<T>( _host.Services )
                .Run( );
        }
    }
}