
using System;
using System.Linq;
using System.Web.Mvc;
using UManage.Components;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace UManage.Controllers
{
    [DnnHandleError]
    public class UManageController : DnnController
    {

        public ActionResult Index()
        {
             
            //var messages = MessageManager.Instance.GetMessages(ModuleContext.ModuleId);

            //var message = MessageManager.Instance.GetDailyMessage(ModuleContext.ModuleId);
            //return View(message);

            return View();

        }

    }

}
