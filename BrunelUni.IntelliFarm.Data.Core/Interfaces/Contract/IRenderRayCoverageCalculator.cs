namespace BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract
{
    public interface IRenderRayCoverageCalculator
    {
        double GetCoverage( int subdivisions );
    }
}