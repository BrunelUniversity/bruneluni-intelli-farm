using System.Text.Json.Serialization;

namespace BrunelUni.IntelliFarm.Data.Core.Dtos
{
    public class RenderOptions
    {
        [ JsonPropertyName( "samples" ) ]
        public int Samples { get; set; }
        [ JsonPropertyName( "max_light_bounces" ) ]
        public int MaxLightBounces { get; set; }
        [ JsonPropertyName( "start_frame" ) ]
        public int StartFrame { get; set; }
        [ JsonPropertyName( "end_frame" ) ]
        public int EndFrame { get; set; }
    }
}