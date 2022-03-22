using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IRenderAnalyser
    {
        BucketDto [ ] GetBuckets( ClientDto [ ] clients, FrameDto [ ] frames );

        double GetPredictedTime( ClientDto client, FrameDto frame );
    }
}