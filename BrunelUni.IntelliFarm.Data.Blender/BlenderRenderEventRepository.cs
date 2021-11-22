﻿using Aidan.Common.Core;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;

namespace BrunelUni.IntelliFarm.Data.Blender
{
    public class BlenderRenderEventRepository : IRenderEventRepository
    {
        private readonly IRenderManagerService _renderManagerService;
        private readonly ISceneProcessor _sceneProcessor;

        public BlenderRenderEventRepository( IRenderManagerService renderManagerService, ISceneProcessor sceneProcessor )
        {
            _renderManagerService = renderManagerService;
            _sceneProcessor = sceneProcessor;
        }
        
        public ObjectResult<RenderResultDto> Create( )
        {
            _sceneProcessor.RunSceneProcessAndExit( _renderManagerService.RenderManager.GetRenderInfo( ).BlendFilePath,
                "logger",
                true );
            return _sceneProcessor.ReadTemp<RenderResultDto>( );
        }
    }
}