
using DotNetNuke.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using DotNetNuke.Data;
using System.Web.Security;

using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using DotNetNuke.Security.Roles;
using System.Collections.Specialized;
using OPSI.UManage;


namespace OPSI.UManage.WebApi
{


    public class UsersController : DnnApiController
    {


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Users_List(string key, string roles, bool deleted, bool unauth, int ResultsPerPage, int CurrentPage, string orderby, string orderclause)
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;

            List<Entities.User_Info> v_List_Users;

            string sql;
            sql = "OPSI_UManage_UsersSearch";

            using (IDataContext ctx = DataContext.Instance())
            {
                v_List_Users = (List<Entities.User_Info>)ctx.ExecuteQuery<Entities.User_Info>(CommandType.StoredProcedure, sql, v_Current_Portal_ID, ResultsPerPage, CurrentPage, key, roles, deleted, unauth, orderby, orderclause);
            }

            string v_json_resulted = Newtonsoft.Json.JsonConvert.SerializeObject(v_List_Users);

            return Request.CreateResponse(HttpStatusCode.OK, v_json_resulted);

        }


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Users_Get(int userid)
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;

            DotNetNuke.Entities.Users.UserInfo v_UserInfo;
            v_UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(v_Current_Portal_ID, userid);

            string v_json_resulted = Newtonsoft.Json.JsonConvert.SerializeObject(v_UserInfo);

            return Request.CreateResponse(HttpStatusCode.OK, v_json_resulted);

        }


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Users_DeleteUnauthorized()
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;

            UserController.DeleteUnauthorizedUsers(v_Current_Portal_ID);

            return Request.CreateResponse(HttpStatusCode.OK, "");

        }


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Users_RemoveDeleted()
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;

            UserController.RemoveDeletedUsers(v_Current_Portal_ID);

            return Request.CreateResponse(HttpStatusCode.OK, "");

        }



        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Users_Update([FromBody]string _json_content)
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;
            HttpResponseMessage resp = new HttpResponseMessage();

            Entities.User_Info v_Received_User = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.User_Info>(_json_content, new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern });

            Entities.User_Update_Result v_Update_Result = new Entities.User_Update_Result();

            if (v_Received_User.UserID > 0)
            {

                DotNetNuke.Entities.Users.UserInfo v_DNN_UserInfo;
                v_DNN_UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(v_Current_Portal_ID, v_Received_User.UserID);

                if (v_DNN_UserInfo != null)
                {

                    v_DNN_UserInfo.Email = v_Received_User.Email;
                    v_DNN_UserInfo.FirstName = v_Received_User.FirstName;
                    v_DNN_UserInfo.LastName = v_Received_User.LastName;
                    v_DNN_UserInfo.DisplayName = v_Received_User.DisplayName;
                    v_DNN_UserInfo.Username = v_Received_User.Username;

                    DotNetNuke.Entities.Users.UserController.UpdateUser(v_Current_Portal_ID, v_DNN_UserInfo);

                    v_Update_Result.errorMessage = "updateok";
                    v_Update_Result.UserID = v_Received_User.UserID;

                }

            }
            else
            {

                //Creating new user
                DotNetNuke.Entities.Users.UserInfo v_DNN_UserInfo = new UserInfo();

                v_DNN_UserInfo.Email = v_Received_User.Email;
                v_DNN_UserInfo.FirstName = v_Received_User.FirstName;
                v_DNN_UserInfo.LastName = v_Received_User.LastName;
                v_DNN_UserInfo.DisplayName = v_Received_User.DisplayName;
                v_DNN_UserInfo.Username = v_Received_User.Username;
                v_DNN_UserInfo.PortalID = v_Current_Portal_ID;

                v_DNN_UserInfo.Profile.PreferredLocale = DotNetNuke.Services.Localization.LocaleController.Instance.GetCurrentLocale(v_Current_Portal_ID).Code;
                v_DNN_UserInfo.Profile.PreferredTimeZone = TimeZoneInfo.Local;
                v_DNN_UserInfo.Profile.FirstName = v_Received_User.FirstName;
                v_DNN_UserInfo.Profile.LastName = v_Received_User.LastName;

                v_DNN_UserInfo.Membership.Password = v_Received_User.Password;
                v_DNN_UserInfo.Membership.Approved = true;
                v_DNN_UserInfo.Membership.CreatedDate = DateTime.Now;

                DotNetNuke.Security.Membership.UserCreateStatus v_CreateResult = DotNetNuke.Entities.Users.UserController.CreateUser(ref v_DNN_UserInfo);
                if (v_CreateResult != DotNetNuke.Security.Membership.UserCreateStatus.Success)
                {

                    v_Update_Result.errorMessage = v_CreateResult.ToString();
                    v_Update_Result.UserID = v_DNN_UserInfo.UserID;

                }
                else
                {
                    v_Update_Result.errorMessage = "creationok";
                    v_Update_Result.UserID = v_DNN_UserInfo.UserID;
                }


            }


            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(v_Update_Result));

        }



        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Users_Authorize(int userid, string mode)
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;

            DotNetNuke.Entities.Users.UserInfo v_UserInfo;
            v_UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(v_Current_Portal_ID, userid);

            if (v_UserInfo != null && mode == "unauthorize")
            {

                v_UserInfo.Membership.Approved = false;
                DotNetNuke.Entities.Users.UserController.UpdateUser(v_Current_Portal_ID, v_UserInfo);

            }
            if (v_UserInfo != null && mode == "authorize")
            {
                v_UserInfo.Membership.Approved = true;
                DotNetNuke.Entities.Users.UserController.UpdateUser(v_Current_Portal_ID, v_UserInfo);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "");

        }



        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Users_Delete(int userid, string mode)
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;

            DotNetNuke.Entities.Users.UserInfo v_UserInfo;
            v_UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(v_Current_Portal_ID, userid);

            if (v_UserInfo != null && mode == "delete")
            {
                DotNetNuke.Entities.Users.UserController.DeleteUser(ref v_UserInfo, false, false);
            }
            if (v_UserInfo != null && mode == "restore")
            {
                DotNetNuke.Entities.Users.UserController.RestoreUser(ref v_UserInfo);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "");

        }


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Users_SendPasswordLink(int userid)
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;

            //Getting Host Settings
            string v_HostSMTPSetting = DotNetNuke.Entities.Controllers.HostController.Instance.GetString("SMTPServer");
            if (String.IsNullOrEmpty(v_HostSMTPSetting))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "smtp-missing");
            }


            DotNetNuke.Entities.Users.UserController v_User_Cont = new UserController();
            DotNetNuke.Entities.Users.UserInfo v_User_Info = v_User_Cont.GetUser(v_Current_Portal_ID, userid);

            if (v_User_Info == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "no-user");
            }

            if (MembershipProviderConfig.PasswordRetrievalEnabled || MembershipProviderConfig.PasswordResetEnabled)
            {
                UserController.ResetPasswordToken(v_User_Info);
            }

            DotNetNuke.Services.Mail.Mail.SendMail(v_User_Info, DotNetNuke.Services.Mail.MessageType.PasswordReminder, PortalSettings);

            return Request.CreateResponse(HttpStatusCode.OK, "ok");

        }





        //[DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        //[HttpGet]
        //[ValidateAntiForgeryToken]
        //[SupportedModules("UManage")]
        //public HttpResponseMessage Users_Delete(int userid)
        //{

        //    int v_Current_Portal_ID = this.ActiveModule.PortalID;

        //    DotNetNuke.Entities.Users.UserInfo v_UserInfo;
        //    v_UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(v_Current_Portal_ID, userid);

        //    if (v_UserInfo != null)
        //    {
        //        bool v_delete_response = DotNetNuke.Entities.Users.UserController.DeleteUser(ref v_UserInfo, false, false);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, "");

        //}


    }

    public class RolesController : DnnApiController
    {




        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Roles_List()
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;

            List<Entities.Role_Info> v_Return_List = new List<Entities.Role_Info>();

            DotNetNuke.Security.Roles.RoleController v_Roles_Controller = new DotNetNuke.Security.Roles.RoleController();
            ArrayList v_Roles_List = v_Roles_Controller.GetPortalRoles(v_Current_Portal_ID);

            foreach (DotNetNuke.Security.Roles.RoleInfo v_Roles_Info in v_Roles_List)
            {

                Entities.Role_Info v_Role_Info_Json = new Entities.Role_Info();
                v_Role_Info_Json.RoleName = v_Roles_Info.RoleName;
                v_Role_Info_Json.ID = v_Roles_Info.RoleID;
                v_Return_List.Add(v_Role_Info_Json);

            }

            string v_json_resulted = Newtonsoft.Json.JsonConvert.SerializeObject(v_Return_List);

            return Request.CreateResponse(HttpStatusCode.OK, v_json_resulted);

        }




        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage Roles_AddRemove(int userid, string role, string mode)
        {

            int v_Current_Portal_ID = this.ActiveModule.PortalID;

            //Getting user info
            DotNetNuke.Entities.Users.UserInfo v_UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(v_Current_Portal_ID, userid);

            if (v_UserInfo != null)
            {

                RoleController v_RoleController = new RoleController();
                RoleInfo v_RoleInfo = v_RoleController.GetRoleByName(v_Current_Portal_ID, role);

                if (v_RoleInfo != null)
                {

                    if (v_UserInfo.IsInRole(v_RoleInfo.RoleName) && mode == "remove")
                    {
                        RoleController.DeleteUserRole(v_UserInfo, v_RoleInfo, this.PortalSettings, false);
                        return Request.CreateResponse(HttpStatusCode.OK, "ok role removed");
                    }

                    if (v_UserInfo.IsInRole(v_RoleInfo.RoleName) == false && mode == "add")
                    {

                        v_RoleController.AddUserRole(v_Current_Portal_ID, userid, v_RoleInfo.RoleID, DateTime.MinValue);
                        return Request.CreateResponse(HttpStatusCode.OK, "ok role added");
                    }

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "No such role with that RoleName");

                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "No such user with that Id");
            }

            return Request.CreateResponse(HttpStatusCode.OK, "ok");

        }


    }





}


