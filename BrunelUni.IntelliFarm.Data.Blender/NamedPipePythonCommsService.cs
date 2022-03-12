using System.Threading;
using BrunelUni.IntelliFarm.Data.Core.Enums;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class NamedPipePythonCommsService : IPythonCommsService
    {
        private readonly INamedPipeState _namedPipeState;

        public NamedPipePythonCommsService( INamedPipeState namedPipeState ) { _namedPipeState = namedPipeState; }
        
        public void SendMessage( string message )
        {
            _namedPipeState.Request = NamePipeOperationEnum.Write;
            _namedPipeState.DataIn = message;
            Cleanup( );
        }

        public string RecieveMessage( )
        {
            _namedPipeState.Request = NamePipeOperationEnum.Read;
            while( _namedPipeState.DataOut == "" )
            {
                Thread.Sleep( 50 );
            }
            Cleanup( );
            return _namedPipeState.DataOut;
        }

        private void Cleanup( )
        {
            _namedPipeState.Request = NamePipeOperationEnum.None;
            _namedPipeState.DataIn = "";
            _namedPipeState.DataOut = "";
        }
    }
}