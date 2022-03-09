using Aidan.Common.Core.Enum;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.Data.API.Controllers
{
    public class RenderDataController : BaseDataController
    {
        private readonly MvcAdapter _mvcAdapter;
        private readonly IRenderManagerService _renderManagerService;
        private readonly ISceneRepository _sceneRepository;

        public RenderDataController( MvcAdapter mvcAdapter, ISceneRepository sceneRepository,
            IRenderManagerService renderManagerService )
        {
            _mvcAdapter = mvcAdapter;
            _sceneRepository = sceneRepository;
            _renderManagerService = renderManagerService;
        }

        [ HttpGet ]
        [ Route( "render-data" ) ]
        public IActionResult GetRenderInfo( )
        {
            var result = _sceneRepository.Read( );
            return result.Status == OperationResultEnum.Success
                ? _mvcAdapter.OkResult( result.Value )
                : _mvcAdapter.BadRequestError( result.Msg );
        }

        [ HttpPut ]
        [ Route( "render-data" ) ]
        public IActionResult UpdateRenderInfo( [ FromBody ] RenderDataDto renderOptions )
        {
            var result = _sceneRepository.Update( renderOptions );
            return result.Status == OperationResultEnum.Success
                ? _mvcAdapter.Success( "file ammended" )
                : _mvcAdapter.BadRequestError( result.Msg );
        }

        [ HttpGet ]
        [ Route( "render-data/coverage" ) ]
        public IActionResult GetCoverage( [ FromBody ] RayCoverageInputDto rayCoverage )
        {
            var result = _sceneRepository.GetCoverage( rayCoverage );
            return _mvcAdapter.OkResult( result );
        }
    }
}