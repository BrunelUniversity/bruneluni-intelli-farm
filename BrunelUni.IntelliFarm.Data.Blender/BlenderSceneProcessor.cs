using System.IO;
using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneProcessor : ISceneProcessor
    {
        private readonly IFileAdapter _fileAdapter;
        private readonly IProcessor _processor;
        private readonly IScriptsRootDirectoryState _scriptsRootDirectoryState;
        private readonly ISerializer _serializer;

        public BlenderSceneProcessor( IProcessor processor,
            ISerializer serializer,
            IFileAdapter fileAdapter,
            IScriptsRootDirectoryState scriptsRootDirectoryState )
        {
            _processor = processor;
            _serializer = serializer;
            _fileAdapter = fileAdapter;
            _scriptsRootDirectoryState = scriptsRootDirectoryState;
        }

        public ObjectResult<T> ReadTemp<T>( ) where T : RenderDto
        {
            var fileResult = _fileAdapter.ReadFile( _scriptsRootDirectoryState.DataScriptsTempFile );
            if( fileResult.Status == OperationResultEnum.Failed )
            {
                return new ObjectResult<T>
                {
                    Status = OperationResultEnum.Failed,
                    Msg = fileResult.Msg
                };
            }

            return new ObjectResult<T>
            {
                Value = _serializer.Deserialize<T>( fileResult.Value ),
                Status = OperationResultEnum.Success
            };
        }

        public Result WriteTemp( RenderDto renderDto )
        {
            var result = _serializer.Serialize( renderDto );
            var fileResult = _fileAdapter.WriteFile( _scriptsRootDirectoryState.DataScriptsTempFile, result );
            return fileResult.Status == OperationResultEnum.Failed ? fileResult : Result.Success( );
        }

        public void ClearTemp( )
        {
            var fileResult = _fileAdapter.WriteFile( _scriptsRootDirectoryState.DataScriptsTempFile, "{}" );
            if( fileResult.Status == OperationResultEnum.Failed )
                throw new IOException( $"file error {fileResult.Msg}" );
        }

        public Result RunSceneProcessAndExit( string pathToBlend, string script, bool render )
        {
            var args = $"{pathToBlend} -b -P {_scriptsRootDirectoryState.DataScriptsDir}\\render.py";
            if( render )
                args += " -a";
            else
                args += $" -- render_{script}";
            var result = _processor.RunAndWait( _scriptsRootDirectoryState.BlenderDirectory, args );
            return result.Status == OperationResultEnum.Failed ? result : Result.Success( );
        }
    }
}