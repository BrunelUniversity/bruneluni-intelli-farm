using System;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Domain
{
    public class RenderAnalyser : IRenderAnalyser
    {
        public(Guid clientId, int [ ] frameNums) [ ] GetFrameNumberBatches(
            (int framenum, double predictedTime, Guid clientId) frameAnaylsis )
        {
            throw new NotImplementedException( );
        }

        public double GetPredictedTime( ClientDto clientDto, FrameDto frameDto ) { throw new NotImplementedException( ); }
    }
}