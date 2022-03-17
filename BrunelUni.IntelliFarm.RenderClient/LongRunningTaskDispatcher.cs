using System;
using System.Threading.Tasks;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public class LongRunningTaskDispatcher : ILongRunningTaskDispatcher
    {
        public void FireAndForget( Action task ) =>
            Task.Run( task );
    }
}