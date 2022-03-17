using System;

namespace BrunelUni.IntelliFarm.RenderClient
{
    public class NavigationService
    {
        public event Action<string> Navigate;

        public void NavigateTo( string dest )
        {
            Navigate?.Invoke( dest );
        }
    }
}