﻿using System.IO;
using Aidan.Common.Core;
using Aidan.Common.Core.Enum;
using Aidan.Common.Core.Interfaces.Contract;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderSceneRepository : ISceneRepository
    {
        private readonly ILoggerAdapter<ISceneRepository> _loggerAdapter;
        private readonly IRenderManagerService _renderManagerService;
        private readonly ISceneProcessor _sceneProcessor;

        public BlenderSceneRepository( ISceneProcessor sceneProcessor, IRenderManagerService renderManagerService,
            ILoggerAdapter<ISceneRepository> loggerAdapter )
        {
            _sceneProcessor = sceneProcessor;
            _renderManagerService = renderManagerService;
            _loggerAdapter = loggerAdapter;
        }


        public Result Update( RenderDataDto renderOptions )
        {
            var fileResult = _sceneProcessor.WriteTemp( renderOptions );
            if( fileResult.Status == OperationResultEnum.Failed ) { return fileResult; }

            var processResult = _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "set_scene_data",
                false );
            _sceneProcessor.ClearTemp( );
            return processResult.Status == OperationResultEnum.Failed ? processResult : Result.Success( );
        }

        public ObjectResult<RenderDataDto> Read( )
        {
            var processResult = _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "get_scene_data",
                false );
            if( processResult.Status == OperationResultEnum.Failed )
            {
                return new ObjectResult<RenderDataDto>
                {
                    Status = OperationResultEnum.Failed,
                    Msg = processResult.Msg
                };
            }

            var readResult = _sceneProcessor.ReadTemp<RenderDataDto>( );
            if( readResult.Status == OperationResultEnum.Failed ) { return readResult; }

            _sceneProcessor.ClearTemp( );
            return new ObjectResult<RenderDataDto>
            {
                Status = OperationResultEnum.Success,
                Value = readResult.Value
            };
        }

        public RayCoverageResultDto GetCoverage( RayCoverageInputDto rayCoverageInputDto )
        {
            _loggerAdapter.LogInfo( $"coverage subdivisions are {rayCoverageInputDto.Subdivisions}" );
            var writeResult = _sceneProcessor.WriteTemp( rayCoverageInputDto );
            if( writeResult.Status == OperationResultEnum.Failed ) throw new IOException( writeResult.Msg );
            var blenderResult = _sceneProcessor.RunSceneProcessAndExit(
                _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "get_scene_and_viewport_coverage", false );
            if( blenderResult.Status == OperationResultEnum.Failed ) throw new IOException( blenderResult.Msg );
            var readResult = _sceneProcessor.ReadTemp<RayCoverageResultDto>( );
            if( readResult.Status == OperationResultEnum.Failed ) throw new IOException( readResult.Msg );
            _sceneProcessor.ClearTemp( );
            return readResult.Value;
        }
    }
}