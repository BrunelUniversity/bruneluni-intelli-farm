using System;
using Aidan.Common.Core;
using Aidan.Common.DependencyInjection;
using Aidan.Common.Utils;
using BrunelUni.IntelliFarm.Data.Core;
using Microsoft.Extensions.DependencyInjection;

namespace BrunelUni.IntelliFarm.Crosscutting.DIModule
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection BindCrosscuttingLayer( this IServiceCollection serviceCollection ) =>
            serviceCollection.BindServices( new Action[]
            {
                CommonUtilsInitializer.Initialize,
                CommonInitializer.Initialize
            }, DataApplicationConstants.CrosscuttingRootNamespace );
    }
}