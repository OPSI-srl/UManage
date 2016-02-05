namespace OPSI.UManage.WebApi.Posts
{

    /// <summary>
    /// Represents a POST data in the User update WebAPI
    /// </summary>
    public class Users_Update
    {

        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }

}
