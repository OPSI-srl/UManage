

using System;
using System.Web.Caching;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using DotNetNuke.Security.Roles;

namespace OPSI.UManage.Entities
{


    public class Role_Info
    {

        public int ID { get; set; }
        public string RoleName { get; set; }

    }

    public class User_Info
    {

        public int UserID{ get; set; }
		public string DisplayName{ get; set; }
        public string Email { get; set; }
		public string  Telephone{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool Authorised { get; set; }
        public string Profile_Picture_FileID { get; set; }

        [ReadOnlyColumn]

        public int TotalRows { get; set; }
        [ReadOnlyColumn]
        public int Pages { get; set; }

        [ReadOnlyColumn]
        public string Profile_Picture_URL {
            get
            {
                string v_return = DotNetNuke.Common.Globals.ApplicationPath + "/images/no_avatar.gif";
                if ( string.IsNullOrWhiteSpace(this.Profile_Picture_FileID) == false )
                {
                    var fileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(int.Parse(Profile_Picture_FileID));
                    if ((fileInfo != null))
                    {
                        v_return = DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(fileInfo);
                    }
                }
                return v_return;
            }
        }

    }

    public class User_Update_Result
    {
        public string errorMessage { get; set; }
        public int UserID { get; set; }

    }

}
