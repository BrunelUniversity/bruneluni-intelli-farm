using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface ICommandIn
    {
        void Run<TIn>( TIn @in ) where TIn : RenderDto;
    }
}