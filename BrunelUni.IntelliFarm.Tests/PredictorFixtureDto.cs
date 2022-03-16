using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Tests
{
    public class PredictorFixtureDto
    {
        public FrameDto Frame { get; set; }
        public double ActualRenderTime { get; set; }

        public override string ToString( ) =>
            $"samples: {Frame.Samples} " +
            $"coverage: {Frame.SceneCoverage} " +
            $"tri_count: {Frame.TriangleCount} " +
            $"viewport_cov: {Frame.ViewportCoverage} " +
            $"bounces: {Frame.MaxDiffuseBounces} " +
            $"actual_render_time: {ActualRenderTime}";
    }
}