using Aidan.Common.Core.Interfaces.Contract;
using Aidan.Common.Utils.Test;
using BrunelUni.IntelliFarm.Data.Blender;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Constants;
using BrunelUni.IntelliFarm.Tests.Unit.Data.Fakes;
using NSubstitute;
using NUnit.Framework;

namespace BrunelUni.IntelliFarm.Tests.Unit.Data.DataAccess.AnimationContextTests.CommandTests
{
    [ TestFixture( true, " -a" ) ]
    [ TestFixture( false, "" ) ]
    public class Given_An_InAndOutCommand_When_Ran : GivenWhenThen<ICommandInAndOut>
    {
        private readonly bool _render;
        private readonly string _endOfArgs;
        private RenderMetaDto _renderMetaDto;
        private ISceneProcessor _sceneProcessor;
        private string _command;
        private ISerializer _serializer;
        private IFileAdapter _fileAdapter;
        private IScriptsRootDirectoryState _scriptsRootDirectoryState;
        private ILoggerAdapter<ISceneProcessor> _loggerAdapter;
        private ScriptsRootDirectoryDto _scriptsRootDirectoryDto;
        private CommandMetaDto _commandMeta;
        private INamedPipeServer _fakeNamePipeServer;

        public Given_An_InAndOutCommand_When_Ran( bool render, string endOfArgs )
        {
            _render = render;
            _endOfArgs = endOfArgs;
        }

        protected override void Given( )
        {
            _fakeNamePipeServer = new FakeNamedPipeServer( );
            _renderMetaDto = new RenderMetaDto( );
            _sceneProcessor = Substitute.For<ISceneProcessor>( );
            _serializer = Substitute.For<ISerializer>( );
            _fileAdapter = Substitute.For<IFileAdapter>( );
            _scriptsRootDirectoryState = new FakeScriptsRootDirectoryState( );
            _loggerAdapter = Substitute.For<ILoggerAdapter<ISceneProcessor>>( );
            _scriptsRootDirectoryDto = _scriptsRootDirectoryState.ScriptsRootDirectoryDto;
            _command = "some-command";
            _commandMeta = new CommandMetaDto
            {
                Command = _command,
                Render = _render,
                RenderMetaDto = _renderMetaDto,
                ScriptsRootDirectoryDto = _scriptsRootDirectoryDto
            };
            SUT = new BlenderCommandInAndOut( _commandMeta,
                _serializer,
                _fakeNamePipeServer,
                _sceneProcessor );
        }

        protected override void When( )
        {
            SUT.Run<RenderDataDto, RenderDataDto>( new RenderDataDto
            {
                DiffuseBounces = 1,
                EndFrame = 2,
                GlossyBounces = 3
            } );
        }

        [ Test ]
        public void Then_Scene_Processor_Is_Ran_With_Correct_Args( )
        {
            _sceneProcessor.Received( ).RunSceneProcess( TestConstants.BlenderDirectory,
                $"-b -P {TestConstants.DataScriptsDir}\\main.py{_endOfArgs}" );
            _sceneProcessor.Received( 1 ).RunSceneProcess( Arg.Any<string>( ), Arg.Any<string>( ) );
        }
    }
}