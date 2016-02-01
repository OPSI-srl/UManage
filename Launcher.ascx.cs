

using System;
using System.Web.UI.WebControls;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using System.Web;

namespace OPSI.UManage.Pages
{

    public partial class Launcher : PortalModuleBase
    {


        protected void Page_Load(object sender, EventArgs e)
        {


            try
            {

                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxAntiForgerySupport();
                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxScriptSupport();


                if (Request["iu"] != null)
                {
                  if (Request["iu"].ToString() != "")
                  {
                    // impersoniamo un caro utonto
                    int uid = int.Parse(Request["iu"].ToString());

                    //UserInfo MyUserInfo = UserController.GetUser(this.PortalId, uid, true);
                    UserInfo MyUserInfo = UserController.GetUserById(this.PortalId, uid);
                    if ((MyUserInfo != null))
                    {
                      //Remove user from cache
                      if (Page.User != null)
                      {
                        DotNetNuke.Common.Utilities.DataCache.ClearUserCache(this.PortalSettings.PortalId, Context.User.Identity.Name);
                      }

                      // sign current user out
                      PortalSecurity objPortalSecurity = new PortalSecurity();
                      objPortalSecurity.SignOut();

                      // sign new user in
                      UserController.UserLogin(PortalId, MyUserInfo, PortalSettings.PortalName, Request.UserHostAddress, false);

                      // redirect to the base url
                      if (HttpContext.Current.Request.IsSecureConnection)
                      {
                        Response.Redirect("https://" + PortalSettings.PortalAlias.HTTPAlias, true);
                      }
                      else
                      {
                        Response.Redirect("http://" + PortalSettings.PortalAlias.HTTPAlias, true);
                      }
                    }
                  }
                }


                //Module is not usuable by unauthenticated users
                if (UserInfo.UserID <= 0)
                {
                    this.panel_unregistereduser.Visible = true;
                    this.panel_normal.Visible = false;
                    return;
                }



                if (this.IsPostBack == false)
                {

                    if (Session["UManage_StopAutoLauncher"] == null)
                    {
                        LaunchModule();
                    }

                }



            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }


        }

        public void lnk_launcher_click(object sender, EventArgs e)
        {

            LaunchModule();

        }


        public void LaunchModule()
        {

            Response.Redirect(EditUrl("", "", "main", "SkinSrc=%5BG%5DSkins%2f_default%2fNo+Skin&ContainerSrc=%5BG%5DContainers%2f_default%2fNo+Container"));

        }


    }


}