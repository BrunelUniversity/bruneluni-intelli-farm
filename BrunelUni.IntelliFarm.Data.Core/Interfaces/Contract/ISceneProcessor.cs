using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ISceneProcessor
    {
        T ReadTemp<T>( ) where T : RenderDto;
        void WriteTemp( RenderDto renderDto );
        void ClearTemp( );
        void RunSceneProcessAndExit( string pathToBlend, string script, bool render );
    }
}