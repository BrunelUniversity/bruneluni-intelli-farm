using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Core.Dtos;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;
using Newtonsoft.Json;

namespace BrunelUni.IntelliFarm.DataAccess
{
    public class IntelliFarmWebClient : IWebClient
    {
        private readonly IConfigurationAdapter _configurationAdapter;
        private readonly IJsonCamelAndPascalCaseSerializer _serializer;
        private readonly ILoggerAdapter<IWebClient> _loggerAdapter;
        private readonly string _baseUrl;

        public IntelliFarmWebClient( IConfigurationAdapter configurationAdapter,
            IJsonCamelAndPascalCaseSerializer serializer,
            ILoggerAdapter<IWebClient> loggerAdapter )
        {
            _configurationAdapter = configurationAdapter;
            _serializer = serializer;
            _loggerAdapter = loggerAdapter;
            _baseUrl = _configurationAdapter.Get<MainAppOptions>( ).ApiBaseUrl;
        }

        public byte[] GetAsBytes( string endpoint )
        {
            var appOptions = _configurationAdapter.Get<MainAppOptions>( );
            using var httpClient = new HttpClient( );
            httpClient.BaseAddress = new Uri( appOptions.ApiBaseUrl );
            
            var task = httpClient.GetAsync( endpoint );
            task.Wait( );
            var responseTask = task.Result.Content.ReadAsByteArrayAsync( );
            responseTask.Wait( );
            return responseTask.Result;
        }

        public string DownloadFile( string endpoint, string filename )
        {
            var path = $"{Directory.GetCurrentDirectory( )}//{filename}";
            using var webClient = new WebClient( );
            webClient.DownloadFile( $"{_baseUrl}{endpoint}", path );
            return path;
        }

        public T Create<T>( string endpoint, T body )
        {
            using var client = new HttpClient( );
            client.BaseAddress = new Uri( _baseUrl );
            var request = new HttpRequestMessage( HttpMethod.Post, endpoint );
            var content = _serializer.Serialize( body );
            _loggerAdapter.LogInfo( $"POST {content}" );
            request.Content = new StringContent( content,
                Encoding.UTF8,
                "application/json" );
            var task = client.SendAsync( request );
            task.Wait( );
            var taskRead = task.Result.Content.ReadAsStringAsync( );
            taskRead.Wait( );
            return JsonConvert.DeserializeObject<T>( taskRead.Result );
        }
    }
}