namespace BrunelUni.IntelliFarm.Data.Core.Dtos
{
    public class BatchModeDto
    {
        public bool BatchMode { get; set; }
        public int PolyMultiplier { set; get; }
        public float CoverageIncrement { set; get; }
        public float PolySteps { set; get; }
        public float CoverageSteps { set; get; }
        public string BasePath { set; get; }
        public float StartPoly { set; get; }
        public float StartCoverage { get; set; }
    }
}