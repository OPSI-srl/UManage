using UManage_Repository.Entities;
using UManage_Repository.Repos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace UManage_Repository.Repos
{
    internal class RolesRepository : BaseRepository, IRolesRepository
    {

        #region "Log"

        public static OPSI_Logger.ErrorLog log = new OPSI_Logger.ErrorLog();

        #endregion

        #region Data Retrieval

        /// <summary>
        /// Returns a list of DNN roles for the given portal Id
        /// </summary>
        /// <param name="portalId">The id of the portal</param>
        /// <returns>A List collection of DNN roles</returns>
        public List<DotNetNuke.Security.Roles.RoleInfo> ListPortalRoles(int portalId)
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
        public void AddRemoveUserRole(int portalId, int userId, string roleName, string mode, DotNetNuke.Entities.Portals.PortalSettings portalSettings)
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
            catch (Exception ex)
            {

                throw ex;

            }

        }

        #endregion

        public virtual void Dispose()
        {
            this.Dispose();
        }

    }

    internal interface IRolesRepository : IDisposable
    {
        List<DotNetNuke.Security.Roles.RoleInfo> ListPortalRoles(int portalId);
        AddRemoveUserRole(int portalId, int userId, string roleName, string mode, DotNetNuke.Entities.Portals.PortalSettings portalSettings);
    }

}

