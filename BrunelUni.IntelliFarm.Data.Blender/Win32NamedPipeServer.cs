using System;
using System.IO.Pipes;
using System.Text;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class Win32NamedPipeServer : INamedPipeServer
    {
        private readonly string _fileName;
        private readonly ILoggerAdapter<INamedPipeServer> _loggerAdapter;
        private NamedPipeServerStream _pipeServer;

        public Win32NamedPipeServer( ILoggerAdapter<INamedPipeServer> loggerAdapter )
        {
            _loggerAdapter = loggerAdapter;
            _fileName = DataApplicationConstants.PipeName;
        }

        public string Send( string message, Action fireAndForgetAfterPipeCreated )
        {
            ServerDo( false, fireAndForgetAfterPipeCreated, message );
            return ServerDo( true );
        }

        private string ServerDo( bool writeRead,
            Action fireAndForget = null,
            string message = null )
        {
            _pipeServer = new NamedPipeServerStream( _fileName, PipeDirection.InOut, 10 );
            _loggerAdapter.LogInfo( "pipe created" );
            if( fireAndForget != null )
            {
                fireAndForget( );
                _loggerAdapter.LogInfo( "fire and forget ran" );
            }

            _loggerAdapter.LogInfo( "server waiting for connection" );
            _pipeServer.WaitForConnection( );
            _loggerAdapter.LogInfo( "server accepted connection" );
            var bytes = new byte[ 200 ];
            if( writeRead )
            {
                _loggerAdapter.LogInfo( "server reading" );
                _pipeServer.Read( bytes );
                _loggerAdapter.LogInfo( "server read" );
            }
            else
            {
                _loggerAdapter.LogInfo( "server writing" );
                _pipeServer.Write( Encoding.UTF8.GetBytes( message ) );
                _loggerAdapter.LogInfo( "server written" );
            }

            _pipeServer.Close( );
            _loggerAdapter.LogInfo( "pipe closed by server" );
            return writeRead ? Encoding.UTF8.GetString( bytes ) : default;
        }
    }
}