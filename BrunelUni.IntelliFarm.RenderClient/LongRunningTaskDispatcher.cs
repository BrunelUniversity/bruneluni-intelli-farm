using System;
using System.Threading.Tasks;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public class LongRunningTaskDispatcher : ILongRunningTaskDispatcher
    {
        private readonly ILoggerAdapter<ILongRunningTaskDispatcher> _loggerAdapter;

        public LongRunningTaskDispatcher( ILoggerAdapter<ILongRunningTaskDispatcher> loggerAdapter )
        {
            _loggerAdapter = loggerAdapter;
        }

        public void FireAndForget( Action task )
        {
            _loggerAdapter.LogInfo( "dispatching long running task..." );
            Task.Run( task );
        }
    }
}