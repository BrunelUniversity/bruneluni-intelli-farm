using System;
using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IRenderAnalyser
    {
        ( Guid clientId, int [ ] frameNums ) [ ] GetFrameNumberBatches(
            ( int framenum, double predictedTime, Guid clientId ) frameAnaylsis );

        double GetPredictedTime( CallibrationDto callibrationDto, FrameMetaData frameData );
    }
}