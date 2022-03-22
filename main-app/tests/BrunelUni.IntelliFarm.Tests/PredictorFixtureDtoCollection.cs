using System.Collections.Generic;

namespace BrunelUni.IntelliFarm.Tests
{
    public class PredictorFixtureDtoCollection : List<PredictorFixtureDto>
    {
        public PredictorFixtureDtoCollection( List<PredictorFixtureDto> list )
        {
            AddRange( list );
        }
        
        public override string ToString( )
        {
            var jobsStr = "";
            foreach( var dto in this )
            {
                jobsStr += $"t: {dto.ActualRenderTime}, ";
            }
            return $"task_no: {this.Count} jobs: [{jobsStr}]";
        }
    }
}