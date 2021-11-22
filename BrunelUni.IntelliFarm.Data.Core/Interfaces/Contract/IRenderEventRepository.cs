using System.ComponentModel;
using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;

namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface IRenderEventRepository
    {
        [ Description( "triggers a render and returns the time taken to render" ) ]
        public ObjectResult<double> Create( RenderEventDto renderOptions );
    }
}