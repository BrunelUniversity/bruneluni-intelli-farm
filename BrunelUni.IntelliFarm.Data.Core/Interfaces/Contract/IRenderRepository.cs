using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface IRenderRepository
    {
        public Result Create( RenderOptions renderOptions );
    }
}