using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneCommandFacade : ISceneCommandFacade
    {
        private readonly ILoggerAdapter<ISceneCommandFacade> _loggerAdapter;
        private readonly IRenderManagerService _renderManagerService;
        private readonly ISceneProcessor _sceneProcessor;

        public BlenderSceneCommandFacade( ISceneProcessor sceneProcessor, IRenderManagerService renderManagerService,
            ILoggerAdapter<ISceneCommandFacade> loggerAdapter )
        {
            _sceneProcessor = sceneProcessor;
            _renderManagerService = renderManagerService;
            _loggerAdapter = loggerAdapter;
        }


        public void SetSceneData( RenderDataDto renderOptions )
        {
            _sceneProcessor.WriteTemp( renderOptions );

            _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "set_scene_data",
                false );
            _sceneProcessor.ClearTemp( );
        }

        public RenderDataDto GetSceneData( )
        {
            _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "get_scene_data",
                false );

            var data = _sceneProcessor.ReadTemp<RenderDataDto>( );
            _sceneProcessor.ClearTemp( );
            return data;
        }

        public RayCoverageResultDto GetSceneAndViewportCoverage( RayCoverageInputDto rayCoverageInputDto )
        {
            _loggerAdapter.LogInfo( $"coverage subdivisions are {rayCoverageInputDto.Subdivisions}" );
            _sceneProcessor.WriteTemp( rayCoverageInputDto );
            _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "get_scene_and_viewport_coverage", false );
            var readResult = _sceneProcessor.ReadTemp<RayCoverageResultDto>( );
            _sceneProcessor.ClearTemp( );
            return readResult;
        }

        public TriangleCountDto GetTriangleCount( ) { return new TriangleCountDto( ); }

        public RenderResultDto Render( )
        {
            _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "render_frame",
                true );
            var fileResult = _sceneProcessor.ReadTemp<RenderResultDto>( );
            _sceneProcessor.ClearTemp( );
            return fileResult;
        }
    }
}