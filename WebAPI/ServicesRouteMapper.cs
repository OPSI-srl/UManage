using DotNetNuke.Web.Api;

namespace OPSI.UManage.Services
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("UManage", "default", "{controller}/{action}", new[] { "OPSI.UManage.WebApi" });
        }
    }
}