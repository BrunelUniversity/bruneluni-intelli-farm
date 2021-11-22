using Aidan.Common.Core.Enum;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.Data.API.Controllers
{
    [ Route( "render-data" ) ]
    public class RenderDataController : BaseDataController
    {
        private readonly MvcAdapter _mvcAdapter;
        private readonly ISceneRepository _sceneRepository;
        private readonly IRenderManagerService _renderManagerService;

        public RenderDataController( MvcAdapter mvcAdapter, ISceneRepository sceneRepository, IRenderManagerService renderManagerService )
        {
            _mvcAdapter = mvcAdapter;
            _sceneRepository = sceneRepository;
            _renderManagerService = renderManagerService;
        }

        [ HttpGet ]
        public IActionResult GetRenderInfo( )
        {
            var result = _sceneRepository.Read( );
            return result.Status == OperationResultEnum.Success ?
                _mvcAdapter.OkResult( result.Value ) :
                _mvcAdapter.BadRequestError( result.Msg );
        }

        [ HttpPut ]
        public IActionResult UpdateRenderInfo( [ FromBody ] RenderDataDto renderOptions )
        {
            var result = _sceneRepository.Update( renderOptions );
            return result.Status == OperationResultEnum.Success ?
                _mvcAdapter.Success( "file ammended" ) :
                _mvcAdapter.BadRequestError( result.Msg );
        }
    }
}