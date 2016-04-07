﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UManage_Repository.Entities
{

    public class UserEntity
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

        //TODO
        public int TotalRows { get; set; }
        public int Pages { get; set; }

        //TODO
        public string Profile_Picture_URL
        {
            get
            {
                string v_return = "";
                //v_return = DotNetNuke.Common.Globals.ApplicationPath + "/images/no_avatar.gif";
                //if (string.IsNullOrWhiteSpace(this.Profile_Picture_FileID) == false)
                //{
                //    var fileInfo = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(int.Parse(Profile_Picture_FileID));
                //    if ((fileInfo != null))
                //    {
                //        v_return = DotNetNuke.Services.FileSystem.FileManager.Instance.GetUrl(fileInfo);
                //    }
                //}
                return v_return;
            }
        }

    }

}
