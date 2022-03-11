using System.IO;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneProcessor : ISceneProcessor
    {
        private readonly IFileAdapter _fileAdapter;
        private readonly ILoggerAdapter<ISceneProcessor> _loggerAdapter;
        private readonly IProcessor _processor;
        private readonly IScriptsRootDirectoryState _scriptsRootDirectoryState;
        private readonly ISerializer _serializer;

        public BlenderSceneProcessor( IProcessor processor,
            ISerializer serializer,
            IFileAdapter fileAdapter,
            IScriptsRootDirectoryState scriptsRootDirectoryState,
            ILoggerAdapter<ISceneProcessor> loggerAdapter )
        {
            _processor = processor;
            _serializer = serializer;
            _fileAdapter = fileAdapter;
            _scriptsRootDirectoryState = scriptsRootDirectoryState;
            _loggerAdapter = loggerAdapter;
        }

        public T ReadTemp<T>( ) where T : RenderDto
        {
            var fileResult = _fileAdapter.ReadFile( _scriptsRootDirectoryState.DataScriptsTempFile );
            if( fileResult.Status == OperationResultEnum.Failed )
            {
                throw new IOException( $"failed to read msg: {fileResult.Msg}" );
            }

            return _serializer.Deserialize<T>( fileResult.Value );
        }

        public void WriteTemp( RenderDto renderDto )
        {
            var result = _serializer.Serialize( renderDto );
            var fileResult = _fileAdapter.WriteFile( _scriptsRootDirectoryState.DataScriptsTempFile, result );
            if( fileResult.Status == OperationResultEnum.Failed )
                throw new IOException( $"failed to write msg: {fileResult.Msg}" );
        }

        public void ClearTemp( )
        {
            var fileResult = _fileAdapter.WriteFile( _scriptsRootDirectoryState.DataScriptsTempFile, "{}" );
            if( fileResult.Status == OperationResultEnum.Failed )
                throw new IOException( $"file error {fileResult.Msg}" );
        }

        public void RunSceneProcessAndExit( string pathToBlend, string script, bool render )
        {
            var args = $"{pathToBlend} -b -P {_scriptsRootDirectoryState.DataScriptsDir}\\main.py";
            if( render )
                args += " -a";
            else
                args += $" -- {script}";
            _loggerAdapter.LogInfo( $"blender args: {args}" );
            var result = _processor.RunAndWait( _scriptsRootDirectoryState.BlenderDirectory, args );
        }
    }
}