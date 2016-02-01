

using System;
using System.Web.UI.WebControls;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using System.Web.UI;
using DotNetNuke.Entities.Users;

namespace OPSI.UManage.Pages
{

    public partial class Main : PortalModuleBase
    {


        protected void Page_Load(object sender, EventArgs e)
        {


            try
            {

                System.Web.UI.ScriptManager.GetCurrent(this.Page).EnablePageMethods = true;

                //Check that in any case this module cannot be accessed by anounymous users
                //set the session variable that will stop auto-loading from launcher
                if (UserInfo.UserID <= 0)
                {
                    Session["UManage_StopAutoLauncher"] = 1;
                    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
                }

                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxAntiForgerySupport();
                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxScriptSupport();

                if (this.IsPostBack == false)
                {

                    VAR_PageBase.Text = DotNetNuke.Common.Globals.NavigateURL();
                    VAR_ModulePath.Text = this.ControlPath;
                    VAR_ProfilePicBasePath.Text = Page.ResolveUrl("~/profilepic.ashx?userid=");

                    VAR_CurrentLanguage.Text = (System.Threading.Thread.CurrentThread.CurrentCulture.Name).Split('-')[0].ToString();
                    VAR_PortalID.Text = PortalId.ToString();

                    var moduleController = new ModuleController();
                    var adminUserModule = moduleController.GetModuleByDefinition(PortalId, "User Accounts");
                    var url = DotNetNuke.Common.Globals.NavigateURL(adminUserModule.TabID, "Edit", "mid=" + adminUserModule.ModuleID, "userId={{userid}}", "popUp=true");

                    VAR_FullEditPath.Text = url;

                    //set the session variable that will stop auto-loading from launcher
                    Session["UManage_StopAutoLauncher"] = 1;

                    if (UserInfo.IsInRole(PortalSettings.AdministratorRoleName))
                    {
                        this.VAR_IsAdmin.Text = "1";
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected string CloseModule_URL()
        {
            return DotNetNuke.Common.Globals.NavigateURL();
        }
    }
}