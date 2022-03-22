using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Utils;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public static class LoggerExtensions
    {
        public static ILoggerAdapter<T> GetLogger<T>( ) where T : class =>
            new WpfLogger<T>( new WindowsFileAdapter( ) );
    }
}