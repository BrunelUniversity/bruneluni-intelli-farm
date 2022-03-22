using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.CommandTests
{
    public class Given_An_OutCommand_When_Ran : GivenWhenThen<ICommandOut>
    {
        private string _command;
        private ICommandInAndOut _commandInAndOut;
        private ICommandInAndOutFactory _commandInAndOutFactory;
        private CommandMetaDto _commandMeta;
        private RenderResultDto _dataOut;
        private bool _render;
        private RenderMetaDto _renderMetaDto;
        private RenderResultDto _result;
        private ScriptsRootDirectoryDto _scriptsRootDirectoryDto;

        protected override void Given( )
        {
            _commandInAndOut = Substitute.For<ICommandInAndOut>( );
            _commandInAndOut.Run<RenderDataDto, RenderResultDto>( Arg.Any<RenderDataDto>( ) )
                .Returns( _dataOut );
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
            _commandInAndOutFactory
                .Factory( Arg.Any<CommandMetaDto>( ) )
                .Returns( _commandInAndOut );
            SUT = new BlenderCommandOut( _commandMeta, _commandInAndOutFactory );
        }

        protected override void When( ) { _result = SUT.Run<RenderResultDto>( ); }

        [ Test ]
        public void Then_Command_In_And_Out_Is_Ran_With_Correct_Args( )
        {
            _commandInAndOutFactory.Received( ).Factory( Arg.Any<CommandMetaDto>( ) );
            _commandInAndOutFactory.Received( 1 ).Factory( _commandMeta );
            _commandInAndOutFactory
                .Factory( _commandMeta )
                .Received( 1 )
                .Run<RenderDataDto, RenderResultDto>( Arg.Any<RenderDataDto>( ) );
            Assert.AreSame( _dataOut, _result );
        }
    }
}