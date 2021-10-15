using System.Text.Json.Serialization;

namespace BrunelUni.IntelliFarm.Data.Core
{
    public class RenderOptions
    {
        [ JsonPropertyName( "samples" ) ]
        public int Samples { get; set; }
        [ JsonPropertyName( "max_light_bounces" ) ]
        public int MaxLightBounces { get; set; }
    }
}