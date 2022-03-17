using System;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public class WpfNavigationService : INavigationService
    {
        public event Action<string> Navigate;

        public void NavigateTo( string dest )
        {
            Navigate?.Invoke( dest );
        }
    }
}