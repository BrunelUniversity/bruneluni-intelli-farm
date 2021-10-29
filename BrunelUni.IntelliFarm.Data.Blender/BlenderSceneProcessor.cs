using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneProcessor : ISceneProcessor
    {
        private readonly IProcessor _processor;
        private readonly ISerializer _serializer;
        private readonly IFileAdapter _fileAdapter;
        
        public BlenderSceneProcessor( IProcessor processor, ISerializer serializer, IFileAdapter fileAdapter )
        {
            _processor = processor;
            _serializer = serializer;
            _fileAdapter = fileAdapter;
        }

        // TODO: validation
        public ObjectResult<T> ReadTemp<T>( ) where T : RenderDto
        {
            var fileResult = _fileAdapter.ReadFile( $"{DataApplicationConstants.DataScriptsTempFile}" );
            return new ObjectResult<T>
            {
                Value = _serializer.Deserialize<T>( fileResult.Value ),
                Status = OperationResultEnum.Success
            };
        }

        public Result WriteTemp( RenderDto renderDto ) { throw new System.NotImplementedException( ); }

        public Result RunSceneProcessAndExit( string pathToBlend, string script, bool render ) { throw new System.NotImplementedException( ); }
    }
}