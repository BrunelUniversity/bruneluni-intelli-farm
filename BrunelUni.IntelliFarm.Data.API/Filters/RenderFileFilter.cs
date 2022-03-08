using System;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrunelUni.IntelliFarm.Data.API.Filters
{
    public class RenderFileFilter : ActionFilterAttribute
    {
        private readonly IAnimationContext _animationContext;
        private readonly MvcAdapter _mvcAdapter;

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

            try { _animationContext.InitializeScene( renderFile ); }
            catch( ArgumentException e )
            {
                context.Result =
                    _mvcAdapter.NotFoundError( e.Message );
            }
        }
    }
}