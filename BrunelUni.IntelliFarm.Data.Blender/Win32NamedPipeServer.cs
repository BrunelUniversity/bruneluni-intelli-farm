using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Enums;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class Win32NamedPipeServer : INamedPipeServer
    {
        private readonly ILoggerAdapter<INamedPipeServer> _loggerAdapter;
        private readonly INamedPipeState _namedPipeState;
        private readonly string _fileName;
        private NamedPipeServerStream _pipeServer;

        public Win32NamedPipeServer( ILoggerAdapter<INamedPipeServer> loggerAdapter,
            INamedPipeState namedPipeState )
        {
            _loggerAdapter = loggerAdapter;
            _namedPipeState = namedPipeState;
            _fileName = "blender-api-pipe";
        }

        public void Start( )
        {
            Task.Run( ( ) =>
            {
                while( true )
                {
                    _pipeServer = new NamedPipeServerStream( _fileName, PipeDirection.InOut, 10 );
                    _loggerAdapter.LogInfo( "server waiting for connection" );
                    _pipeServer.WaitForConnection( );
                    _loggerAdapter.LogInfo( "client connection accepted" );
                    if( _namedPipeState.Request == NamePipeOperationEnum.Read )
                    {
                        _loggerAdapter.LogInfo( "server reading" );
                        var bytes = new byte[ 200 ];
                        _pipeServer.Read( bytes );
                        _loggerAdapter.LogInfo( "server read" );
                        _namedPipeState.DataOut = Encoding.UTF8.GetString( bytes );
                    }
                    else if( _namedPipeState.Request == NamePipeOperationEnum.Write )
                    {
                        _loggerAdapter.LogInfo( "server writing" );
                        _pipeServer.Write( Encoding.UTF8.GetBytes( _namedPipeState.DataIn ) );
                        _loggerAdapter.LogInfo( "server written" );
                    }
                    _pipeServer.Close( );
                }
            } );
        }
    }
}