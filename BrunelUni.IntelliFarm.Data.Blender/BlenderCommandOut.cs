using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderCommandOut : ICommandOut
    {
        private readonly ICommandInAndOutFactory _commandInAndOutFactory;
        private readonly CommandMetaDto _commandMetaDto;

        public BlenderCommandOut( CommandMetaDto commandMetaDto,
            ICommandInAndOutFactory commandInAndOutFactory )
        {
            _commandMetaDto = commandMetaDto;
            _commandInAndOutFactory = commandInAndOutFactory;
        }

        // TODO: best workaround for now, maybe add constructable base render dto
        public TOut Run<TOut>( ) where TOut : RenderDto
        {
            return _commandInAndOutFactory
                .Factory( _commandMetaDto )
                .Run<RenderDataDto, TOut>( new RenderDataDto( ) );
        }
    }
}