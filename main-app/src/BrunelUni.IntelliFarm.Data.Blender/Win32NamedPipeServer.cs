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
        private readonly ILoggerAdapter<INamedPipeServer> _loggerAdapter;
        private NamedPipeServerStream _pipeReadServer;
        private NamedPipeServerStream _pipeWriteServer;

        public Win32NamedPipeServer( ILoggerAdapter<INamedPipeServer> loggerAdapter )
        {
            _loggerAdapter = loggerAdapter;
        }

        public string Send( string message, Action fireAndForgetAfterPipeCreated )
        {
            _pipeWriteServer = new NamedPipeServerStream( DataApplicationConstants.PipeNameWrite,
                PipeDirection.InOut,
                10 );
            _pipeReadServer = new NamedPipeServerStream( DataApplicationConstants.PipeNameRead,
                PipeDirection.InOut,
                10 );
            fireAndForgetAfterPipeCreated( );
            _loggerAdapter.LogInfo( $"server waiting for connection on {DataApplicationConstants.PipeNameWrite}" );
            _pipeWriteServer.WaitForConnection( );
            _loggerAdapter.LogInfo( $"client connected on {DataApplicationConstants.PipeNameWrite}" );
            _loggerAdapter.LogInfo( "server writing" );
            _loggerAdapter.LogInfo( $"server writing message: {message}" );
            _pipeWriteServer.Write( Encoding.UTF8.GetBytes( message ) );
            _loggerAdapter.LogInfo( "server written" );
            _loggerAdapter.LogInfo( $"server waiting for connection on {DataApplicationConstants.PipeNameRead}" );
            _pipeReadServer.WaitForConnection( );
            _loggerAdapter.LogInfo( $"client connected on {DataApplicationConstants.PipeNameRead}" );
            _loggerAdapter.LogInfo( "server reading" );
            var bytes = new byte[ 200 ];
            _pipeReadServer.Read( bytes );
            _loggerAdapter.LogInfo( "server read" );
            _pipeReadServer.Close( );
            _loggerAdapter.LogInfo( $"{DataApplicationConstants.PipeNameRead} pipe closed by server" );
            _pipeWriteServer.Close( );
            _loggerAdapter.LogInfo( $"{DataApplicationConstants.PipeNameWrite} pipe closed by server" );
            return Encoding.UTF8.GetString( bytes );
        }
    }
}