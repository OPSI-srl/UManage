using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Web.Api;

namespace OPSI.UManage.WebApi
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("UManage", "default", "{controller}/{action}", new[] { "OPSI.UManage.WebApi" });
        }
    }
}