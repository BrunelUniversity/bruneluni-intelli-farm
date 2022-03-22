using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderCommandIn : ICommandIn
    {
        private readonly ICommandInAndOutFactory _commandInAndOutFactory;
        private readonly CommandMetaDto _commandMetaDto;

        public BlenderCommandIn( CommandMetaDto commandMetaDto,
            ICommandInAndOutFactory commandInAndOutFactory )
        {
            _commandMetaDto = commandMetaDto;
            _commandInAndOutFactory = commandInAndOutFactory;
        }

        public void Run<TIn>( TIn @in ) where TIn : RenderDto
        {
            _commandInAndOutFactory
                .Factory( _commandMetaDto )
                .Run<TIn, RenderDataDto>( @in );
        }
    }
}