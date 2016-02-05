using System;
using System.Linq;
using System.Collections.Generic;


namespace OPSI.UManage.Business.Controllers
{

    /// <summary>
    /// Business class containing the functions related to Roles management.
    /// </summary>
    public static class DNN_Roles
    {

        /// <summary>
        /// Returns a list of DNN roles for the given portal Id
        /// </summary>
        /// <param name="portalId">The id of the portal</param>
        /// <returns>A List collection of DNN roles</returns>
        public static List<DotNetNuke.Security.Roles.RoleInfo> ListPortalRoles(int portalId)
        {

            List<DotNetNuke.Security.Roles.RoleInfo> result = null;

            try
            {

                //Using DNN API to return portal roles
                result = DotNetNuke.Security.Roles.RoleController.Instance.GetRoles(portalId).ToList();

            }
            catch (Exception ex)
            {

                throw ex;

            }
            
            return result;

        }


        /// <summary>
        /// Function used to add or remove a role from a DNN user
        /// </summary>
        /// <param name="portalId">the current portal id</param>
        /// <param name="userId">the user id to manage roles of</param>
        /// <param name="roleName">the role name to add or remove</param>
        /// <param name="mode">"add", to add a role. "remove" to remove a role</param>
        /// <param name="portalSettings">the portal settings for the current portal</param>
        public static void AddRemoveUserRole(int portalId, int userId, string roleName, string mode, DotNetNuke.Entities.Portals.PortalSettings portalSettings)
        {


            try
            {

                //Getting user info
                DotNetNuke.Entities.Users.UserInfo userInfo = DotNetNuke.Entities.Users.UserController.GetUserById(portalId, userId);

                //Checking if the user actually exists
                if (userInfo != null)
                {

                    //Getting the role data
                    DotNetNuke.Security.Roles.RoleInfo roleInfo = DotNetNuke.Security.Roles.RoleController.Instance.GetRoleByName(portalId, roleName);

                    //Checking that the role exists
                    if (roleInfo != null)
                    {

                        if (userInfo.IsInRole(roleInfo.RoleName) && mode == "remove")
                        {

                            //removes the role
                            DotNetNuke.Security.Roles.RoleController.DeleteUserRole(userInfo, roleInfo, portalSettings, false);

                        }
                        else if (userInfo.IsInRole(roleInfo.RoleName) == false && mode == "add")
                        {

                            //adds the role
                            DotNetNuke.Security.Roles.RoleController.Instance.AddUserRole(portalId, userId, roleInfo.RoleID, DotNetNuke.Security.Roles.RoleStatus.Approved, false, DateTime.Now, DateTime.MinValue);
                        }

                    }
                    else
                    {

                        //No role with that name was found

                    }

                }
                else
                {

                    //The user specified was not found

                }

            }
            catch(Exception ex)
            {

                throw ex;

            }

        }
        
    }

}
