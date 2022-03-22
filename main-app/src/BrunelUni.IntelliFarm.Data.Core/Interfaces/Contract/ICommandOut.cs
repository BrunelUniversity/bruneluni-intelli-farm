using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ICommandOut
    {
        TOut Run<TOut>( ) where TOut : RenderDto;
    }
}