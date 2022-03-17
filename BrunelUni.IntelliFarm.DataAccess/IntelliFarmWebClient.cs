using System;
using System.IO;
using System.Net;
using System.Net.Http;
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
            var appOptions = _configurationAdapter.Get<MainAppOptions>( );
            using var webClient = new WebClient( );
            webClient.DownloadFile( $"{appOptions.ApiBaseUrl}{endpoint}", path );
            return path;
        }
    }
}