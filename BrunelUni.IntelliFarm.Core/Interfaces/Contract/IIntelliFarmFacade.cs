using Aidan.Common.Core;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IIntelliFarmFacade
    {
        Result CreateProject( string name, string filePath, params string [ ] devices );
        void CreateDevice( string deviceName );
        void Render( string sceneName, string deviceName );
    }
}