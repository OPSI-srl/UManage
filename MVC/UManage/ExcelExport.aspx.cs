using DotNetNuke.Data;
//using OPSI.UManage.Entities;
using UManage_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UManage
{
    public partial class ExcelExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string v_Current_Portal_ID = Request["pid"];
            string ResultsPerPage = Request["rpp"];
            string CurrentPage = Request["cp"];
            string key = Request["key"];
            string roles = Request["roles"];
            string deleted = Request["deleted"];
            string unauth = Request["unauth"];
            string orderby = Request["orderby"];
            string orderclause = Request["orderclause"]; 

            string attachment = "attachment; filename=UManage_Exported_Users_" + DateTime.UtcNow.Ticks.ToString() + ".xls";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            var sb = new System.Text.StringBuilder();
            
            List<UserEntity> v_List_Users;

            string sql;
            sql = "OPSI_UManage_UsersSearch";

            using (IDataContext ctx = DataContext.Instance())
            {
                v_List_Users = (List<UserEntity>)ctx.ExecuteQuery<UserEntity>(CommandType.StoredProcedure, sql, v_Current_Portal_ID, 
                                                                                                                ResultsPerPage, 
                                                                                                                CurrentPage, 
                                                                                                                key, 
                                                                                                                roles, 
                                                                                                                deleted, 
                                                                                                                unauth, 
                                                                                                                orderby, 
                                                                                                                orderclause);
            }

            string _Content = "";

            sb.Append("<table>");

            sb.Append("<tr>");
            sb.Append("<td><b>DNN User Name</b></td>");
            sb.Append("<td><b>Name</b></td>");
            sb.Append("<td><b>Surname</b></td>");
            sb.Append("<td><b>Email</b></td>");
            sb.Append("<td><b>Display Name</b></td>");
            sb.Append("<td><b>Creation Date</b></td>");
            sb.Append("<td><b>Last Access</b></td>");
            sb.Append("</tr>");
            

            foreach (UserEntity _Info in v_List_Users)
            {
                _Content = AddLine(_Info);
                sb.AppendLine(_Content);

            }

            sb.Append("</table>");

            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        public string AddLine(UserEntity info)
        {

            string _result = "<tr>";

            _result += "<td>" + info.Username  + "</td>";
            _result += "<td>" + info.FirstName + "</td>";
            _result += "<td>" + info.LastName + "</td>";
            _result += "<td>" + info.Email + "</td>";
            _result += "<td>" + info.DisplayName + "</td>";
            _result += "<td>" + info.CreatedOnDate + "</td>";
            _result += "<td>" + info.LastLoginDate + "</td>";

            _result += "</tr>";
            return _result;
        }
    }
}