using System;
using Aidan.Common.DependencyInjection;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core;
using Microsoft.Extensions.DependencyInjection;

namespace BrunelUni.IntelliFarm.Data.DIModule
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection BindDataLayer( this IServiceCollection serviceCollection ) =>
            serviceCollection.BindServices( new Action[]
            {
                DataInitializer.Initialize,
                DataBlenderInitializer.Initialize
            }, DataApplicationConstants.RootNamespace );
    }
}