using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneRepository : ISceneRepository
    {
        private readonly IProcessor _processor;
        private readonly ISerializer _serializer;
        private readonly IFileAdapter _fileAdapter;

        public BlenderSceneRepository( IProcessor processor,
            ISerializer serializer,
            IFileAdapter fileAdapter)
        {
            _processor = processor;
            _serializer = serializer;
            _fileAdapter = fileAdapter;
        }
        

        public Result Update( RenderDataDto renderOptions )
        {
            var content = _serializer.Serialize( renderOptions );
            var fileResult = _fileAdapter.WriteFile( DataApplicationConstants.DataScriptsTempFile, content );
            if( fileResult.Status == OperationResultEnum.Failed )
            {
                return Result.Error( fileResult.Msg );
            }
            var blenderResult = _processor.RunAndWait( "blender", $"-b -P {DataApplicationConstants.DataScriptsDir}\\render_writer.py" );
            return blenderResult.Status == OperationResultEnum.Failed ? Result.Error( blenderResult.Msg ) : Result.Success(  );
        }

        // TODO: implement
        public ObjectResult<RenderDataDto> Read( ) => new ObjectResult<RenderDataDto>
            { Status = OperationResultEnum.Failed, Msg = "implement" };
    }
}