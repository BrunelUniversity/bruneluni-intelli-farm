using Aidan.Common.Core.Enum;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrunelUni.IntelliFarm.Data.API.Filters
{
    public class BlenderInstallFilter : ActionFilterAttribute
    {
        private readonly IAnimationContext _animationContext;
        private readonly MvcAdapter _mvcAdapter;

        public BlenderInstallFilter( IAnimationContext animationContext, MvcAdapter mvcAdapter )
        {
            _animationContext = animationContext;
            _mvcAdapter = mvcAdapter;
        }
        
        public override void OnActionExecuting( ActionExecutingContext context )
        {
            // if( _fileAdapter.Exists( _scriptsRootDirectoryState.BlenderDirectory ).Status == OperationResultEnum.Success )
            // {
            //     return;
            // }
            // _webClientAdapter.DownloadFile( $"{DataApplicationConstants.BlenderBaseUrl}/{DataApplicationConstants.BlenderVersion}.zip",
            //     $"{_scriptsRootDirectoryState.Directory}\\blender.zip" );
            // _zipAdapter.ExtractToDirectory( $"{_scriptsRootDirectoryState.Directory}\\blender.zip",
            //     $"{_scriptsRootDirectoryState.Directory}\\blender" );
            var result = _animationContext.Initialize( );
            if( result.Status == OperationResultEnum.Failed )
            {
                context.Result = _mvcAdapter.BadRequestError( result.Msg );
            }
        }
    }
}