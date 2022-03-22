using System;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface INamedPipeServer
    {
        string Send( string message, Action fireAndForgetAfterPipeCreated );
    }
}