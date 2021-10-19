using BrunelUni.IntelliFarm.Data.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BrunelUni.IntelliFarm.Data.API.Controllers
{
    [ ServiceFilter( typeof( RenderFileFilter ), Order = 1 ) ]
    public abstract class BaseDataController : ControllerBase
    {
    }
}