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
    /// Decorate every WebAPI with this attribute to execute security checking 
    /// </summary>
    public sealed class ValidateOPSIToken : ActionFilterAttribute
    {

        #region CONST

        private bool settingEnableSecurity = bool.Parse(ConfigurationManager.AppSettings["OPSI_Security_Enable"]);

        #endregion


        /// <summary>
        /// This method checks for the token to be valid
        /// </summary>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

            if (settingEnableSecurity)
            {

                //For logging purpouses
                string actionName = string.Empty;
                string controllerName = string.Empty;
                string tokenValue = string.Empty;

                try
                {

                    actionName = actionContext.ActionDescriptor.ActionName;
                    controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                    if (actionContext.Request.Headers.Contains("opsitoken"))
                    {

                        tokenValue = actionContext.Request.Headers.GetValues("opsitoken").FirstOrDefault();

                        OPSI_Logger.ErrorLog logSuccess = new OPSI_Logger.ErrorLog();
                        logSuccess.Info("TokenReceived:" + tokenValue, "WEB API: " + controllerName + "\\" + actionName);

                        //getting token values
                        Token tokenInfo = new Token("", tokenValue);
                        if (!tokenInfo.Validate())
                        {
                            //the token is not valid
                            OPSI_Logger.ErrorLog log = new OPSI_Logger.ErrorLog();
                            log.Info(controllerName + "\\" + actionName + " Invalid Token ", "WEB API: " + controllerName + "\\" + actionName);
                            HttpResponseMessage responseInvalid = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "token_invalid");
                            actionContext.Response = responseInvalid;
                        }

                    }
                    else
                    {

                        //no headers "opsitoken" was found
                        OPSI_Logger.ErrorLog log = new OPSI_Logger.ErrorLog();
                        log.Info(controllerName + "\\" + actionName + " No Token Error ", "WEB API: " + controllerName + "\\" + actionName);
                        HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "no_opsi_token");
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


        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {

            if (settingEnableSecurity)
            {

                string tokenValue = string.Empty;
                Token tokenInfo = null;

                try
                {

                    //getting token values
                    tokenValue = actionContext.Request.Headers.GetValues("opsitoken").FirstOrDefault();
                    tokenInfo = new Token("", tokenValue);

                    tokenInfo.UpdateTokensValues();

                    if (actionContext.Response.Content != null)
                    {

                        if (actionContext.Response.Content.Headers.Contains("Access-Control-Expose-Headers"))
                        {
                            actionContext.Response.Content.Headers.Remove("Access-Control-Expose-Headers");
                        }
                        actionContext.Response.Content.Headers.Add("Access-Control-Expose-Headers", "opsitoken");

                        if (actionContext.Response.Content.Headers.Contains("opsitoken"))
                        {
                            actionContext.Response.Content.Headers.Remove("opsitoken");
                        }

                    }

                }
                catch (Exception ex)
                {

                    //OPSI_Logger.ErrorLog log = new OPSI_Logger.ErrorLog();
                    //log.Info(controllerName + "\\" + actionName + " Error: " + ex.InnerException + ex.StackTrace, "WEB API: " + controllerName + "\\" + actionName);
                    HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message + "-" + ex.StackTrace);
                    actionContext.Response = response;

                }


                if (actionContext.Response.Content != null)
                {
                    actionContext.Response.Content.Headers.Add("opsitoken", tokenInfo.Token_EncryptedValue);
                }

            }

        }


    }

}
