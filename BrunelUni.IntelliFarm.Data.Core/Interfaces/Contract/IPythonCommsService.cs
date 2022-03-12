namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface IPythonCommsService
    {
        public void SendMessage( string message );
        public string RecieveMessage( );
    }
}