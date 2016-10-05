using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;

namespace EDIPrivate.Controllers
{
    [NonController]
    public sealed class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action) =>
            string.Equals(routeContext.HttpContext.Request?.Headers["X-Requested-With"], "XMLHttpRequest") || false;
    }
}