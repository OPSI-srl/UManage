using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Configuration;

namespace OPSI_Security
{

    /// <summary>
    /// Decorate every public WebAPI with this attribute to protect against DOS attacks
    /// </summary>
    public sealed class ProtectedDOSAttacks : ActionFilterAttribute
    {

        #region CONST

        private bool settingEnableSecurity = bool.Parse(ConfigurationManager.AppSettings["OPSI_Security_Enable"]);

        #endregion

        /// <summary>
        /// This method checks the request to be ok within time and interval allowed
        /// </summary>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

            string actionName = string.Empty;
            string controllerName = string.Empty;
            bool result = true;

            if (settingEnableSecurity)
            {

                try
                {

                    actionName = actionContext.ActionDescriptor.ActionName;
                    controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                    //builds the object to write in the file
                    OPSI_Security.CallersLogger callersLogger = new OPSI_Security.CallersLogger();
                    OPSI_Security.CallersLoggerEntry callersLoggerEntry = new OPSI_Security.CallersLoggerEntry();

                    //getting IP address
                    var myRequest = ((System.Web.HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
                    var ip = myRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ip))
                    {
                        string[] ipRange = ip.Split(',');
                        int le = ipRange.Length - 1;
                        string trueIP = ipRange[le];
                    }
                    else
                    {
                        ip = myRequest.ServerVariables["REMOTE_ADDR"];
                    }
                    callersLoggerEntry.IPAddress = ip;

                    //getting Agent
                    var headers = myRequest.Headers.GetValues("User-Agent");
                    var userAgent = string.Join(" ", headers);
                    callersLoggerEntry.Agent = userAgent;

                    callersLoggerEntry.Time = DateTime.Now;
                    callersLoggerEntry.WebApiName = controllerName + "/" + actionName;
                    result = callersLogger.Write_Caller_Info(callersLoggerEntry);

                    //Too many calls limit detected, blocks execution
                    if (!result)
                    {
                        HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "Too many requests in such little time...");
                        actionContext.Response = response;
                    }


                }
                catch (Exception ex)
                {

                    OPSI_Logger.ErrorLog log = new OPSI_Logger.ErrorLog();
                    log.Info(controllerName + "\\" + actionName + " Error: " + ex.InnerException + ex.StackTrace, "WEB API: " + controllerName + "\\" + actionName);
                    HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.StackTrace);
                    actionContext.Response = response;

                }

            }

            base.OnActionExecuting(actionContext);

        }





    }

}
