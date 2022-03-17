using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BrunelUni.IntelliFarm.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.DataAccess
{
    public class S3RemoteFileService : IRemoteFileService
    {
        private const string IntelliFarmS3Name = "intelli-farm";
        private const string S3Region = "eu-west-2";
        private readonly string _id;
        private readonly string _secret;

        public S3RemoteFileService( string id, string secret )
        {
            _id = id;
            _secret = secret;
        }
        
        public string Get( string path )
        {
            using var client = new AmazonS3Client(
                _id,
                _secret,
                RegionEndpoint.GetBySystemName( S3Region ) );
            var task = client.GetObjectAsync( new GetObjectRequest
            {
                BucketName = IntelliFarmS3Name,
                Key = path
            } );
            task.Wait( );
            var resp = task.Result.ResponseStream;

            using var sr = new StreamReader( resp );

            return sr.ReadToEnd( );
        }

        public string Write( string path ) { throw new System.NotImplementedException( ); }
    }
}