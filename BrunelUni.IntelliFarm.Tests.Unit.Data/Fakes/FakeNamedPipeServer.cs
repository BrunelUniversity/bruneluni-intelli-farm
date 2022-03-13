using System;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using NSubstitute;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.Fakes
{
    public class FakeNamedPipeServer : INamedPipeServer
    {
        public INamedPipeServer Spy { get; } = Substitute.For<INamedPipeServer>( );

        public string Send( string message, Action fireAndForgetAfterPipeCreated )
        {
            fireAndForgetAfterPipeCreated( );
            return Spy.Send( message, fireAndForgetAfterPipeCreated );
        }
    }
}