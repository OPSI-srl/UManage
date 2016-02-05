using System;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace OPSI.UManage.Business.Entities
{

    /// <summary>
    /// Used to represent a User in the search
    /// </summary>
    public class UserListItem
    {

        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
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

    }

}
