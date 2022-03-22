namespace BrunelUni.IntelliFarm.Data.Core.Dtos
{
    public class RenderDataDto : RenderDto
    {
        public int Samples { get; set; }
        public int TransparentMaxBounces { get; set; }
        public int DiffuseBounces { get; set; }
        public int GlossyBounces { get; set; }
        public int TransmissionBounces { get; set; }
        public int VolumeBounces { get; set; }
        public int MaxBounces { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
    }
}