using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Utils;
using BrunelUni.IntelliFarm.Crosscutting.DIModule;
using BrunelUni.IntelliFarm.Data.DIModule;
using BrunelUni.IntelliFarm.Tests.Feasability.Data.SamplesTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Feasability.Data
{
    public abstract class Run_Study<T> where T : IRunnable
    {
        private IHost _host;

        [ SetUp ]
        public void Setup( )
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

        [ Test ]
        public void Run_Study_Test( )
        {
            Assert.AreEqual( "", "" );
        }
    }
}