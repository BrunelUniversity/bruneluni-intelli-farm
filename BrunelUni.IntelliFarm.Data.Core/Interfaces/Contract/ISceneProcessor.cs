using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ISceneProcessor
    {
        ObjectResult<RenderDto> ReadTemp( );
        Result WriteTemp( RenderDto renderDto );
        Result RunSceneProcessAndExit( string pathToBlend, string script, bool render );
    }
}