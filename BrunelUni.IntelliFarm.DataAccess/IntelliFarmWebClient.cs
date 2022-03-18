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

        public string DownloadFile( string endpoint, string filename )
        {
            var path = $"{Directory.GetCurrentDirectory( )}//{filename}";
            using var webClient = new WebClient( );
            webClient.DownloadFile( $"{_baseUrl}{endpoint}", path );
            return path;
        }

        public string UploadFile( string endpoint, string filename )
        {
            var path = $"{Directory.GetCurrentDirectory( )}//{filename}";
            using var client = new HttpClient( );
            client.BaseAddress = new Uri( _baseUrl );
            client.DefaultRequestHeaders.Accept.Clear( );

            var content = new MultipartFormDataContent( );
            var fileContent = new ByteArrayContent( File.ReadAllBytes( path ) );
            content.Add( fileContent, "file", filename );

            var postTask = client.PostAsync( endpoint, content );
            postTask.Wait( );
            var response = postTask.Result;
            var readStringTask = response
                .Content
                .ReadAsStringAsync( );
            readStringTask.Wait( );
            return readStringTask.Result;
        }

        public WebDto Create<T>( string endpoint, T body )
        {
            using var client = new HttpClient( );
            client.BaseAddress = new Uri( _baseUrl );
            var request = new HttpRequestMessage( HttpMethod.Post, endpoint );
            var content = _serializer.Serialize( body );
            _loggerAdapter.LogInfo( $"POST /{endpoint} with body: {content}" );
            request.Content = new StringContent( content,
                Encoding.UTF8,
                "application/json" );
            var task = client.SendAsync( request );
            task.Wait( );
            var taskResult = task.Result;
            var taskRead = taskResult.Content.ReadAsStringAsync( );
            taskRead.Wait( );
            var rawData = taskRead.Result;
            var statusCode = taskResult.StatusCode;
            var webResult = new WebDto
            {
                Data = JsonConvert.DeserializeObject( rawData ),
                StatusCode = statusCode
            };
            _loggerAdapter.LogInfo( $"{statusCode} {rawData}" );
            return webResult;
        }

        public WebDto Get( string endpoint )
        {
            using var client = new HttpClient( );
            client.BaseAddress = new Uri( _baseUrl );
            var request = new HttpRequestMessage( HttpMethod.Get, $"{endpoint}" );
            _loggerAdapter.LogInfo( $"GET /{endpoint}" );
            var task = client.SendAsync( request );
            task.Wait( );
            var taskResult = task.Result;
            var taskRead = taskResult.Content.ReadAsStringAsync( );
            taskRead.Wait( );
            var rawData = taskRead.Result;
            var statusCode = taskResult.StatusCode;
            var webResult = new WebDto
            {
                Data = JsonConvert.DeserializeObject( rawData ),
                StatusCode = statusCode
            };
            _loggerAdapter.LogInfo( $"{statusCode} {rawData}" );
            return webResult;
        }
    }
}