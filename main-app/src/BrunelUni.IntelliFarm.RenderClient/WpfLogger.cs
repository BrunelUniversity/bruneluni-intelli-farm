using Aidan.Common.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public class WpfLogger<T> : ILoggerAdapter<T> where T : class
    {
        private readonly IFileAdapter _fileAdapter;
        private readonly string _logsFile;

        public WpfLogger( IFileAdapter fileAdapter )
        {
            _fileAdapter = fileAdapter;
            _logsFile = $"{fileAdapter?.GetCurrentDirectory( ).Value}\\current-logs.txt";
        }
        
        public void LogInfo( string message )
        {
            var output = $"{typeof(T).Name} INFO: {message}\n";
            var content = _fileAdapter.ReadFile( _logsFile ).Value;
            content += $"\n{output}";
            _fileAdapter.WriteFile( _logsFile, content );
        }

        public void LogDebug( string message )
        {
            var output = $"{typeof(T).Name} DEBUG: {message}\n";
            var content = _fileAdapter.ReadFile( _logsFile ).Value;
            content += $"\n{output}";
            _fileAdapter.WriteFile( _logsFile, content );
        }

        public void LogError( string message )
        {
            var output = $"{typeof(T).Name} ERROR: {message}\n";
            var content = _fileAdapter.ReadFile( _logsFile ).Value;
            content += $"\n{output}";
            _fileAdapter.WriteFile( _logsFile, content );
        }
    }
}