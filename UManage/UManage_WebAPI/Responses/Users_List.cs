using System;
using DotNetNuke.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OPSI.UManage.WebApi.Responses
{

    /// <summary>
    /// Class that represent the result of the user search webapi
    /// </summary>
    class Users_List : Base
    {

        public List<Users_List_UserInfo> Users { get; set; }

        public Users_List()
        {
            this.Users = new List<Users_List_UserInfo>();
        }

        #region "Utils Classes"

        public class Users_List_UserInfo
        {

            public int UserID { get; set; }
            public string DisplayName { get; set; }
            public string Email { get; set; }
            public string Telephone { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }
            //public string Password { get; set; }
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
            public string Profile_Picture_URL
            {
                get
                {
                    string v_return = DotNetNuke.Common.Globals.ApplicationPath + "/images/no_avatar.gif";
                    if (string.IsNullOrWhiteSpace(this.Profile_Picture_FileID) == false)
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

        #endregion

    }

}
