using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.DataAccess
{
    public class IntelliFarmWebClient : IWebClient
    {
        private readonly IConfigurationAdapter _configurationAdapter;
        private readonly ISerializer _serializer;

        public IntelliFarmWebClient( IConfigurationAdapter configurationAdapter,
            ISerializer serializer )
        {
            _configurationAdapter = configurationAdapter;
            _serializer = serializer;
        }

        public TOut Send<TOut, TBody>( HttpMethod httpMethod,
            string endpoint,
            TBody optionalBody = default )
        {
            var appOptions = _configurationAdapter.Get<MainAppOptions>( );
            using var httpClient = new HttpClient( );
            httpClient.BaseAddress = new Uri( appOptions.ApiBaseUrl );
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue( "application/json" ) );

            var task = httpClient.GetAsync( endpoint );
            task.Wait( );
            var responseTask = task.Result.Content.ReadAsStringAsync( );
            responseTask.Wait( );
            return _serializer.Deserialize<TOut>( responseTask.Result );
        }
    }
}