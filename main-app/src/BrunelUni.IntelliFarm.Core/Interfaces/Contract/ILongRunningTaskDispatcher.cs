using System;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface ILongRunningTaskDispatcher
    {
        void FireAndForget( Action task );
    }
}