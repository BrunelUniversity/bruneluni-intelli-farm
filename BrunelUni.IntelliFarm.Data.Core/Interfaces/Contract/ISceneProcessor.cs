using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ISceneProcessor
    {
        ObjectResult<T> ReadTemp<T>( ) where T : RenderDto;
        Result WriteTemp( RenderDto renderDto );
        void ClearTemp( );
        Result RunSceneProcessAndExit( string pathToBlend, string script, bool render );
    }
}