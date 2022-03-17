using System.Net.Http;

namespace BrunelUni.IntelliFarm.Core.Interfaces.Contract
{
    public interface IWebClient
    {
        public TOut Send<TOut, TBody>( HttpMethod httpMethod,
            string endpoint,
            TBody optionalBody = default );
    }
}