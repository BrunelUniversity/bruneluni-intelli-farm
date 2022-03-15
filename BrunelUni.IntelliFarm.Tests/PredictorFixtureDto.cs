using BrunelUni.IntelliFarm.Core.Dtos;

namespace BrunelUni.IntelliFarm.Tests
{
    public class PredictorFixtureDto
    {
        public FrameMetaData FrameMetaData { get; set; }
        public double ActualRenderTime { get; set; }

        public override string ToString( ) =>
            $"samples: {FrameMetaData.Samples} " +
            $"coverage: {FrameMetaData.SceneCoverage} " +
            $"tri_count: {FrameMetaData.TriangleCount} " +
            $"viewport_cov: {FrameMetaData.ViewportCoverage} " +
            $"bounces: {FrameMetaData.MaxDiffuseBounces} " +
            $"actual_render_time: {ActualRenderTime}";
    }
}