namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IIntelliFarmFacade
    {
        void CreateProject( string name, string filePath, string [ ] devices );
        void CreateDevice( string deviceName );
        void Render( );
    }
}