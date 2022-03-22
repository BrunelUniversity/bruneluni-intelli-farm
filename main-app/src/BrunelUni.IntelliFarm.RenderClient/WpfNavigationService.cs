using System;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public class WpfNavigationService : INavigationService
    {
        private readonly ILoggerAdapter<INavigationService> _loggerAdapter;

        public WpfNavigationService( ILoggerAdapter<INavigationService> loggerAdapter )
        {
            _loggerAdapter = loggerAdapter;
        }
        
        public event Action<string> Navigate;

        public void NavigateTo( string dest )
        {
            _loggerAdapter.LogInfo( $"navigating to {dest}" );
            Navigate?.Invoke( dest );
        }
    }
}