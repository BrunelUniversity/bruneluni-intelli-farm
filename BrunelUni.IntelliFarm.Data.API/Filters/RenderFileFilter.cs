using System.IO;
using Aidan.Common.Utils.Web;
using BrunelUni.IntelliFarm.Data.Core.Dtos;
using BrunelUni.IntelliFarm.Data.Core.Interfaces.Contract;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrunelUni.IntelliFarm.Data.API.Filters
{
    public class RenderFileFilter : ActionFilterAttribute
    {
        private readonly MvcAdapter _mvcAdapter;
        private readonly IRenderManagerFactory _renderManagerFactory;
        private readonly IRenderManagerGetter _renderManagerGetter;

        public RenderFileFilter( MvcAdapter mvcAdapter,
            IRenderManagerFactory renderManagerFactory,
            IRenderManagerGetter renderManagerGetter )
        {
            _mvcAdapter = mvcAdapter;
            _renderManagerFactory = renderManagerFactory;
            _renderManagerGetter = renderManagerGetter;
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

            if( !File.Exists( renderFile ) )
            {
                context.Result =
                    _mvcAdapter.NotFoundError(
                        $"'{renderFile}' does not exist" );
                return;
            }
            _renderManagerGetter.RenderManager = _renderManagerFactory.Factory( new RenderMetaDto{ BlendFilePath = renderFile } );
        }
    }
}