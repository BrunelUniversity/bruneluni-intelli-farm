using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IIntelliFarmFacade
    {
        Result CreateProject( string name, string filePath, params string [ ] devices );
        void CreateDevice( string deviceName );
        Result Render( string sceneName, string deviceName );
        void CreateBucketsFromProject( SceneDto sceneDto, string file );
    }
}