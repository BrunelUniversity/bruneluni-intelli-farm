using System.Net;

namespace BrunelUni.IntelliFarm.Core.Dtos
{
    public class WebDto
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
    }
}