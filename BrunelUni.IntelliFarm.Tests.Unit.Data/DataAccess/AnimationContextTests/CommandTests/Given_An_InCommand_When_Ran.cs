using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.CommandTests
{
    public class Given_An_InCommand_When_Ran : GivenWhenThen<ICommandIn>
    {
        private string _command;
        private ICommandInAndOutFactory _commandInAndOutFactory;
        private CommandMetaDto _commandMeta;
        private RenderDataDto _dataIn;
        private bool _render;
        private RenderMetaDto _renderMetaDto;
        private ScriptsRootDirectoryDto _scriptsRootDirectoryDto;

        protected override void Given( )
        {
            _renderMetaDto = new RenderMetaDto( );
            _scriptsRootDirectoryDto = new ScriptsRootDirectoryDto( );
            _command = "some-command";
            _commandMeta = new CommandMetaDto
            {
                Command = _command,
                Render = _render,
                RenderMetaDto = _renderMetaDto,
                ScriptsRootDirectoryDto = _scriptsRootDirectoryDto
            };
            _commandInAndOutFactory = Substitute.For<ICommandInAndOutFactory>( );
            SUT = new BlenderCommandIn( _commandMeta, _commandInAndOutFactory );
        }

        protected override void When( )
        {
            _dataIn = new RenderDataDto( );
            SUT.Run( _dataIn );
        }

        [ Test ]
        public void Then_Command_In_And_Out_Is_Ran_With_Correct_Args( )
        {
            _commandInAndOutFactory.Received( ).Factory( Arg.Any<CommandMetaDto>( ) );
            _commandInAndOutFactory.Received( 1 ).Factory( _commandMeta );
            _commandInAndOutFactory.Factory( _commandMeta ).Received( 1 )
                .Run<RenderDataDto, RenderDataDto>( Arg.Any<RenderDataDto>( ) );
            _commandInAndOutFactory.Factory( _commandMeta ).Received( ).Run<RenderDataDto, RenderDataDto>( _dataIn );
        }
    }
}