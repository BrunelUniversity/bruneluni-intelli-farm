using System.Diagnostics;
using Aidan.Common.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public class WpfLogger<T> : ILoggerAdapter<T> where T : class
    {
        public void LogInfo( string message )
        {
            Trace.WriteLine($"{typeof(T).Name} INFO: {message}");
        }

        public void LogDebug( string message )
        {
            Trace.WriteLine($"{typeof(T).Name} DEBUG: {message}");
        }

        public void LogError( string message )
        {
            Trace.WriteLine($"{typeof(T).Name} ERROR: {message}");
        }
    }
}