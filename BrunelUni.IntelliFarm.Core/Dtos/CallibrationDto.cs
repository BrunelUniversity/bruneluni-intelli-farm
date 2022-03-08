namespace BrunelUni.IntelliFarm.Core.Dtos;

public class CallibrationDto
{
    public RenderTimeTinyType TimeFor0PolyViewpoint { get; set; }
    public RenderTimeTinyType TimeFor80Poly100Coverage { get; set; }
    public PercentageTinyType[] CoveragePerFrame { get; set; }
}