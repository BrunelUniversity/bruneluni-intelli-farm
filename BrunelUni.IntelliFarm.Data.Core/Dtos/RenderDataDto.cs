namespace BrunelUni.IntelliFarm.Data.Core.Dtos
{
    public class RenderDataDto : RenderDto
    {
        public int Samples { get; set; }
        public int MaxBounces { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
    }
}