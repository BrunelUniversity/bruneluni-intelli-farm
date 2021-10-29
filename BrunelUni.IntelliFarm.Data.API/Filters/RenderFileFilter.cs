using Aidan.Common.Core.Enum;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrunelUni.IntelliFarm.Data.API.Filters
{
    public class RenderFileFilter : ActionFilterAttribute
    {
        private readonly MvcAdapter _mvcAdapter;
        private readonly IAnimationContext _animationContext;

        public RenderFileFilter( MvcAdapter mvcAdapter,
            IAnimationContext animationContext )
        {
            _mvcAdapter = mvcAdapter;
            _animationContext = animationContext;
        }
        
        public override void OnActionExecuting( ActionExecutingContext context )
        {
            var renderFile = context.HttpContext.Request.Query[ ApiConstants.RenderFileField ].ToString( );

            if( renderFile == string.Empty )
            {
                context.Result =
                    _mvcAdapter.UnauthorizedError( 
                        $"'{ApiConstants.RenderFileField}' parameter was not present in the request" );
                return;
            }

            var result = _animationContext.Initialize( renderFile );
            
            if( result.Status == OperationResultEnum.Failed )
            {
                context.Result =
                    _mvcAdapter.NotFoundError( result.Msg );
            }
        }
    }
}