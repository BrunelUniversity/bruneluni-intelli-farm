using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ICommandInAndOut
    {
        TOut Run<TIn, TOut>( TIn @in )
            where TIn : RenderDto
            where TOut : RenderDto;
    }
}