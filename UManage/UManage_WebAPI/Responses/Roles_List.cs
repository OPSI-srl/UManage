using System.Collections.Generic;

namespace OPSI.UManage.WebApi.Responses
{

    /// <summary>
    /// Class that represent the result of the roles list webapi
    /// </summary>
    public class Roles_List : Base
    {

        public List<Roles_List_RoleInfo> Roles { get; set; }

        public Roles_List()
        {
            this.Roles = new List<Roles_List_RoleInfo>();
        }

    }

    #region "Utils Classes"

    public class Roles_List_RoleInfo
    {

        public int Id { get; set; }
        public string RoleName { get; set; }

    }

    #endregion

}
