
using DotNetNuke.Web.Api;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;



namespace OPSI.UManage.WebApi
{
    
    public class RolesController : DnnApiController
    {

        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage RolesList()
        {

            //response result
            Responses.Roles_List response = new Responses.Roles_List();

            try
            {
                                
                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //Getting portal roles for the current portal
                UManage_Repository.Services.RolesService rolesService = new UManage_Repository.Services.RolesService();
                List<DotNetNuke.Security.Roles.RoleInfo> roleList = rolesService.ListPortalRoles(portalId);

                //Building response including only necessary info
                foreach (DotNetNuke.Security.Roles.RoleInfo roleInfo in roleList)
                {

                    response.Roles.Add(new Responses.Roles_List_RoleInfo() { Id = roleInfo.RoleID, RoleName = roleInfo.RoleName });

                }

                //positive response
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                response.ErrorMessage = "error";
                response.Success = false;

            }
          
            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }

        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage RolesAddRemove(int userid, string role, string mode)
        {

            //response result
            Responses.Base response = new Responses.Base();

            try
            {

                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //Getting portal roles for the current portal
                UManage_Repository.Services.RolesService rolesService = new UManage_Repository.Services.RolesService();
                rolesService.AddRemoveUserRole(portalId, userid, role, mode, this.PortalSettings);

                //positive response
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                response.ErrorMessage = "error";
                response.Success = false;

            }

            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));


        }


    }

}
