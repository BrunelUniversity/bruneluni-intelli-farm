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

        public override void OnActionExecuting( ActionExecutingContext context ) { _animationContext.Initialize( ); }
    }
}