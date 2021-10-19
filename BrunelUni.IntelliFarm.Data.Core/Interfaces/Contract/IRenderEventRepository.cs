using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface IRenderEventRepository
    {
        public Result Create( RenderEventDto renderOptions );
    }
}