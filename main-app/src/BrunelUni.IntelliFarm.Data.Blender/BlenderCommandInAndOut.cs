using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderCommandInAndOut : ICommandInAndOut
    {
        private readonly CommandMetaDto _commandMetaDto;
        private readonly INamedPipeServer _namedPipeServer;
        private readonly ISceneProcessor _sceneProcessor;
        private readonly IJsonSnakeCaseSerializer _serializer;

        public BlenderCommandInAndOut( CommandMetaDto commandMetaDto,
            IJsonSnakeCaseSerializer serializer,
            INamedPipeServer namedPipeServer,
            ISceneProcessor sceneProcessor )
        {
            _commandMetaDto = commandMetaDto;
            _serializer = serializer;
            _namedPipeServer = namedPipeServer;
            _sceneProcessor = sceneProcessor;
        }

        public TOut Run<TIn, TOut>( TIn @in ) where TIn : RenderDto where TOut : RenderDto
        {
            var dataIn = new DataInDto
            {
                Command = _commandMetaDto.Command,
                Data = @in
            };
            var result = _serializer.Serialize( dataIn );
            var response = _namedPipeServer.Send( result, ( ) =>
            {
                var args = "";
                if( _commandMetaDto.RenderMetaDto.BlendFilePath != null )
                    args = $"{_commandMetaDto.RenderMetaDto.BlendFilePath} ";
                args += $"-b -P {_commandMetaDto.ScriptsRootDirectoryDto.DataScriptsDir}\\main.py";
                if( _commandMetaDto.Render ) args += " -a";
                
                _sceneProcessor.RunSceneProcess( _commandMetaDto.ScriptsRootDirectoryDto.BlenderDirectory, args );
            } );
            return _serializer.Deserialize<TOut>( response );
        }
    }
}