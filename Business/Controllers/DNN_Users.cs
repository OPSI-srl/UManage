using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Data;
using System.Data;

namespace OPSI.UManage.Business.Controllers
{

    /// <summary>
    /// Business class that deals with functions for the DNN users
    /// </summary>
    public static class DNN_Users
    {

        /// <summary>
        /// Searches the portal Users using the Stored supplied.
        /// </summary>
        /// <param name="portalId">The current portal Id</param>
        /// <param name="ResultsPerPage">The number of items to return per page</param>
        /// <param name="CurrentPage">Current page of results</param>
        /// <param name="searchKey">Search parameter (user input)</param>
        /// <param name="roles">Roles filters</param>
        /// <param name="deleted">Set to True, to have Deleted users included in results</param>
        /// <param name="unauth">Set to True, to have Unauth users included in results</param>
        /// <param name="orderby">order by column name</param>
        /// <param name="orderclause">asc or desc</param>
        /// <returns></returns>
        public static List<Business.Entities.UserListItem> ListUsers
            (
                int portalId,
                int ResultsPerPage,
                int CurrentPage,
                string searchKey,
                string roles,
                bool deleted,
                bool unauth,
                string orderby,
                string orderclause
            )
        {

            List<Business.Entities.UserListItem> result = null;

            try
            {

                //Stored procedure name;
                string sql = "OPSI_UManage_UsersSearch";

                //Using Poco to retrieve users with the SP supplied
                using (IDataContext ctx = DataContext.Instance())
                {
                    result = (List<Business.Entities.UserListItem>)ctx.ExecuteQuery<Entities.UserListItem>(CommandType.StoredProcedure, sql, portalId, ResultsPerPage, CurrentPage, searchKey, roles, deleted, unauth, orderby, orderclause);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;

        }


        /// <summary>
        /// Function to return a single DNN user
        /// </summary>
        /// <param name="userId">The id of the user to return</param>
        /// <param name="portalId">The portal id for the user to return</param>
        /// <returns></returns>
        public static DotNetNuke.Entities.Users.UserInfo GetUser(int userId, int portalId)
        {

            DotNetNuke.Entities.Users.UserInfo userInfo = null;

            try
            {

                //Using DNN api to retrive user info
                userInfo = DotNetNuke.Entities.Users.UserController.GetUserById(portalId, userId);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userInfo;

        }


        /// <summary>
        /// Deletes the users which are set as unauthorized
        /// </summary>
        /// <param name="portalId">The portal id to delete users from</param>
        public static void DeleteUnauthorizedUsers(int portalId)
        {

            try
            {

                //Using DNN API
                DotNetNuke.Entities.Users.UserController.DeleteUnauthorizedUsers(portalId);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        /// <summary>
        /// Removes the users which are set as deleted
        /// </summary>
        /// <param name="portalId">The portal id to remove users from</param>
        public static void RemoveDeletedUsers(int portalId)
        {

            try
            {

                //Using DNN API
                DotNetNuke.Entities.Users.UserController.RemoveDeletedUsers(portalId);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Creates or updates a portal user
        /// </summary>
        /// <param name="user">The object containing the information of the user</param>
        /// <param name="portalId">The current portal id</param>
        /// <returns>An object containing the response of the add/update process</returns>
        public static Responses.UpdateUser UpdateUser(Business.Entities.User user, int portalId)
        {

            Responses.UpdateUser updateUserResult = new Responses.UpdateUser();

            try
            {

                //Checking if the user must be updates or created
                if (user.UserID > 0)
                {

                    //The user must be updated, retrieve the existing user first
                    DotNetNuke.Entities.Users.UserInfo v_DNN_UserInfo;
                    v_DNN_UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(portalId, user.UserID);

                    //Check if the user has been found
                    if (v_DNN_UserInfo != null)
                    {

                        //Modifing the DNN user
                        v_DNN_UserInfo.Email = user.Email;
                        v_DNN_UserInfo.FirstName = user.FirstName;
                        v_DNN_UserInfo.LastName = user.LastName;
                        v_DNN_UserInfo.DisplayName = user.DisplayName;
                        v_DNN_UserInfo.Username = user.Username;

                        DotNetNuke.Entities.Users.UserController.UpdateUser(portalId, v_DNN_UserInfo);

                        //Sets a positive response
                        updateUserResult.responseMessage = "updateok";
                        updateUserResult.userId = user.UserID;

                    }

                }
                else
                {

                    //Creating new user
                    DotNetNuke.Entities.Users.UserInfo v_DNN_UserInfo = new DotNetNuke.Entities.Users.UserInfo();

                    //User base info
                    v_DNN_UserInfo.Email = user.Email;
                    v_DNN_UserInfo.FirstName = user.FirstName;
                    v_DNN_UserInfo.LastName = user.LastName;
                    v_DNN_UserInfo.DisplayName = user.DisplayName;
                    v_DNN_UserInfo.Username = user.Username;
                    v_DNN_UserInfo.PortalID = portalId;

                    //Membership
                    v_DNN_UserInfo.Membership.Password = user.Password;
                    v_DNN_UserInfo.Membership.Approved = true;
                    v_DNN_UserInfo.Membership.CreatedDate = DateTime.Now;

                    //DNN API to create user
                    DotNetNuke.Security.Membership.UserCreateStatus v_CreateResult = DotNetNuke.Entities.Users.UserController.CreateUser(ref v_DNN_UserInfo);

                    //Checking response
                    if (v_CreateResult != DotNetNuke.Security.Membership.UserCreateStatus.Success)
                    {

                        //Negative response
                        updateUserResult.responseMessage = v_CreateResult.ToString();
                        updateUserResult.userId = v_DNN_UserInfo.UserID;

                    }
                    else
                    {

                        //Updating profile data (needed for First name, Last name...)
                        v_DNN_UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(portalId, v_DNN_UserInfo.UserID);
                        v_DNN_UserInfo.Profile.PreferredLocale = DotNetNuke.Services.Localization.LocaleController.Instance.GetCurrentLocale(portalId).Code;
                        v_DNN_UserInfo.Profile.PreferredTimeZone = TimeZoneInfo.Local;
                        v_DNN_UserInfo.Profile.FirstName = user.FirstName;
                        v_DNN_UserInfo.Profile.LastName = user.LastName;
                        DotNetNuke.Entities.Users.UserController.UpdateUser(portalId, v_DNN_UserInfo);

                        //Positive response
                        updateUserResult.responseMessage = "creationok";
                        updateUserResult.userId = v_DNN_UserInfo.UserID;

                    }


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return updateUserResult;

        }


        /// <summary>
        /// Changes the authorization status for an user
        /// </summary>
        /// <param name="userId">the id of the user to change</param>
        /// <param name="portalId">the current portal id</param>
        /// <param name="mode">"authorize" or "unauthorize"</param>
        public static void ChangeAuthorizedStatus(int userId, int portalId, string mode)
        {

            try
            {

                //getting the existing user
                DotNetNuke.Entities.Users.UserInfo userInfo;
                userInfo = DotNetNuke.Entities.Users.UserController.GetUserById(portalId, userId);

                if (userInfo != null && mode == "unauthorize")
                {

                    //Changing the auth status and calling the API to change it
                    userInfo.Membership.Approved = false;
                    DotNetNuke.Entities.Users.UserController.UpdateUser(portalId, userInfo);

                }
                if (userInfo != null && mode == "authorize")
                {
                    //Changing the auth status and calling the API to change it
                    userInfo.Membership.Approved = true;
                    DotNetNuke.Entities.Users.UserController.UpdateUser(portalId, userInfo);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Changes the delete status for an user
        /// </summary>
        /// <param name="userId">the id of the user to change</param>
        /// <param name="portalId">the current portal id</param>
        /// <param name="mode">"delete" or "restore"</param>
        public static void ChangeDeleteStatus(int userId, int portalId, string mode)
        {

            try
            {

                //getting the existing user
                DotNetNuke.Entities.Users.UserInfo userInfo;
                userInfo = DotNetNuke.Entities.Users.UserController.GetUserById(portalId, userId);

                if (userInfo != null && mode == "delete")
                {
                    
                    //Calling the API to delete the user
                    DotNetNuke.Entities.Users.UserController.DeleteUser(ref userInfo, false, false);

                }
                if (userInfo != null && mode == "restore")
                {

                    //Calling the API to restore the user
                    DotNetNuke.Entities.Users.UserController.RestoreUser(ref userInfo);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Function to Send the email with the password reset link
        /// </summary>
        /// <param name="portalId">The current portal id</param>
        /// <param name="userId">The user id to retrieve the password of</param>
        /// <param name="portalSettings">The current portal settings</param>
        public static void SendPasswordResetLink(int portalId, int userId, DotNetNuke.Entities.Portals.PortalSettings portalSettings)
        {

            try
            {

                //Getting Host Settings
                string hostSMTPSetting = DotNetNuke.Entities.Controllers.HostController.Instance.GetString("SMTPServer");
                if (String.IsNullOrEmpty(hostSMTPSetting))
                {
                    //SMTP not found
                    return;
                }

                DotNetNuke.Entities.Users.UserController usersController = new DotNetNuke.Entities.Users.UserController();
                DotNetNuke.Entities.Users.UserInfo userInfo = usersController.GetUser(portalId, userId);

                if (userInfo == null)
                {
                    //User not found
                    return;
                }

                //Checking portal settings
                if (DotNetNuke.Security.Membership.MembershipProviderConfig.PasswordRetrievalEnabled || DotNetNuke.Security.Membership.MembershipProviderConfig.PasswordResetEnabled)
                {
                    //Resetting password
                    DotNetNuke.Entities.Users.UserController.ResetPasswordToken(userInfo);
                }

                //Sending the email
                DotNetNuke.Services.Mail.Mail.SendMail(userInfo, DotNetNuke.Services.Mail.MessageType.PasswordReminder, portalSettings);

            }
            catch (Exception ex)
            {
                throw ex;
            }
                        

        }


    }

}
