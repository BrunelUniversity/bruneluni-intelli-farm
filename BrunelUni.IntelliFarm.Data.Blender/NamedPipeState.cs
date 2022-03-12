using BrunelUni.IntelliFarm.Data.Core.Enums;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class NamedPipeState : INamedPipeState
    {
        private readonly object _nessMonster;
        private NamePipeOperationEnum _request;
        private string _dataIn;
        private string _dataOut;

        public NamedPipeState( )
        {
            _nessMonster = new object( );
        }

        public NamePipeOperationEnum Request
        {
            get
            {
                lock( _nessMonster )
                {
                    return _request;
                }
            }
            set
            {
                lock( _nessMonster )
                {
                    _request = value;
                }
            }
        }

        public string DataIn
        {
            get
            {
                lock( _nessMonster )
                {
                    return _dataIn;
                }
            }
            set
            {
                lock( _nessMonster )
                {
                    _dataIn = value;
                }
            }
        }

        public string DataOut
        {
            get
            {
                lock( _nessMonster )
                {
                    return _dataOut;
                }
            }
            set
            {
                lock( _nessMonster )
                {
                    _dataOut = value;
                }
            }
        }
    }
}