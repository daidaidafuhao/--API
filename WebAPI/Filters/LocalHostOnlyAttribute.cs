using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WebAPI.Filters
{
    public class LocalHostOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
            var localIp = context.HttpContext.Connection.LocalIpAddress;

            // 检查是否为本机访问
            if (remoteIp != null)
            {
                // 检查是否为localhost (::1 for IPv6) 或 127.0.0.1
                if (!IPAddress.IsLoopback(remoteIp) && 
                    !remoteIp.Equals(localIp))
                {
                    context.Result = new StatusCodeResult(403); // Forbidden
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}