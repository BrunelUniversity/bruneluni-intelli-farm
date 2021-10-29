using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Contract;
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
        
        // TODO: implement
        public Result Update( RenderDataDto renderOptions ) => Result.Error( "implement" );

        // TODO: implement
        public ObjectResult<RenderDataDto> Read( ) => new ObjectResult<RenderDataDto>
            { Status = OperationResultEnum.Failed, Msg = "implement" };
    }
}