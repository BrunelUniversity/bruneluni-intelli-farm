using System;
using Aidan.Common.Core.Attributes;
using Aidan.Common.Core.Enum;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    [ Service(Scope = ServiceLifetimeEnum.Singleton) ]
    public interface INavigationService
    {
        event Action<string> Navigate;

        void NavigateTo( string dest );
    }
}