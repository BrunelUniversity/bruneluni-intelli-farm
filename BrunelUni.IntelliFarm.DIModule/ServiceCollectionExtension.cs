using System;
using Aidan.Common.DependencyInjection;
using BrunelUni.IntelliFarm.Core;
using BrunelUni.IntelliFarm.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace BrunelUni.IntelliFarm.DIModule
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection BindIntelliFarm( this IServiceCollection serviceCollection ) =>
            serviceCollection.BindServices( new Action [ ]
            {
                MainInitializer.Initialize,
                DataAccessInitializer.Initialize
            }, ApplicationConstants.RootNamespace );
    }
}