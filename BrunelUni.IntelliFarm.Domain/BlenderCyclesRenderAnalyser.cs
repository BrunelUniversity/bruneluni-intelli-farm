using System;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Domain
{
    public class BlenderCyclesRenderAnalyser : IRenderAnalyser
    {
        public(Guid clientId, int [ ] frameNums) [ ] GetFrameNumberBatches(
            (int framenum, double predictedTime, Guid clientId) frameAnaylsis )
        {
            throw new NotImplementedException( );
        }

        public double GetPredictedTime( CallibrationDto callibrationDto, FrameMetaData frameData ) { throw new NotImplementedException( ); }
    }
}