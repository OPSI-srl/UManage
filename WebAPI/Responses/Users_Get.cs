
namespace OPSI.UManage.WebApi.Responses
{

    /// <summary>
    /// Class that represent the result of the user get single webapi
    /// </summary>
    public class Users_Get : Base
    {

        public DotNetNuke.Entities.Users.UserInfo user { get; set; }

    }

}
