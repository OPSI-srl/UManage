namespace OPSI.UManage.WebApi.Responses
{

    /// <summary>
    /// Class that represent the result of the user update webapi
    /// </summary>
    public class Users_Update : Base
    {

        public int UserID { get; set; }
        public string responseMessage { get; set; }

    }

}
