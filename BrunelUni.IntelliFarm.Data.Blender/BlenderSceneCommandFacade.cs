using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneCommandFacade : ISceneCommandFacade
    {
        private readonly ICommandInAndOutFactory _commandInAndOutFactory;
        private readonly ICommandInFactory _commandInFactory;
        private readonly ICommandOutFactory _commandOutFactory;
        private readonly IRenderManagerService _renderManagerService;
        private readonly IScriptsRootDirectoryState _scriptsRootDirectoryState;

        public BlenderSceneCommandFacade( ICommandInFactory commandInFactory,
            IRenderManagerService renderManagerService,
            ICommandOutFactory commandOutFactory,
            ICommandInAndOutFactory commandInAndOutFactory,
            IScriptsRootDirectoryState scriptsRootDirectoryState )
        {
            _commandInFactory = commandInFactory;
            _renderManagerService = renderManagerService;
            _commandOutFactory = commandOutFactory;
            _commandInAndOutFactory = commandInAndOutFactory;
            _scriptsRootDirectoryState = scriptsRootDirectoryState;
        }

        public void SetSceneData( RenderDataDto renderOptions )
        {
            _commandInFactory
                .Factory( CreateMeta( "set_scene_data", false ) )
                .Run( renderOptions );
        }

        public RenderDataDto GetSceneData( )
        {
            return _commandOutFactory
                .Factory( CreateMeta( "get_scene_data", false ) )
                .Run<RenderDataDto>( );
        }

        public RayCoverageResultDto GetSceneAndViewportCoverage( RayCoverageInputDto rayCoverageInputDto )
        {
            return _commandInAndOutFactory
                .Factory( CreateMeta( "get_scene_and_viewport_coverage", false ) )
                .Run<RayCoverageInputDto, RayCoverageResultDto>( rayCoverageInputDto );
        }

        public TriangleCountDto GetTriangleCount( )
        {
            return _commandOutFactory
                .Factory( CreateMeta( "get_triangles_count", false ) )
                .Run<TriangleCountDto>( );
        }

        public RenderResultDto Render( )
        {
            return _commandOutFactory
                .Factory( CreateMeta( "render_frame", true ) )
                .Run<RenderResultDto>( );
        }

        private CommandMetaDto CreateMeta( string commandName, bool render )
        {
            return new CommandMetaDto
            {
                Command = commandName,
                Render = render,
                RenderMetaDto = _renderManagerService.RenderManager.GetRenderInfo( ),
                ScriptsRootDirectoryDto = _scriptsRootDirectoryState.ScriptsRootDirectoryDto
            };
        }
    }
}