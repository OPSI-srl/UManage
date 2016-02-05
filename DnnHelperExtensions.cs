using DotNetNuke.Entities.Portals;
using DotNetNuke.UI.Modules;
using DotNetNuke.Web.Razor.Helpers;
using System.Web;

public static class DnnHelperExtensions
{
    public static System.Web.UI.Page Page(this DnnHelper helper)
    {
        var page = HttpContext.Current.CurrentHandler as System.Web.UI.Page;
        return page;
    }
}