using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace OPSI_Logger
{

    public class ErrorLog
    {

        protected string connectionString = "";

        public ErrorLog()
        {
            this.connectionString = ConfigurationManager.AppSettings["OPSILog"];
        }

        public void Info(string UserMessage)
        {
            string setting = ConfigurationManager.AppSettings["loginfo"];
            Exception Ex = new Exception();
            Write(UserMessage, "Info", Ex, "", setting);
        }

        public void Info(string UserMessage, string MethodName)
        {
            string setting = ConfigurationManager.AppSettings["loginfo"];
            Exception Ex = new Exception();
            Write(UserMessage, "Info", Ex, MethodName, setting);

        }

        public void Warning(string UserMessage)
        {
            string setting = ConfigurationManager.AppSettings["logwarning"];
            Exception Ex = new Exception();
            Write(UserMessage, "Warning", Ex, "", setting);
        }

        public void Warning(string UserMessage, string MethodName)
        {
            string setting = ConfigurationManager.AppSettings["logwarning"];
            Exception Ex = new Exception();
            Write(UserMessage, "Warning", Ex, MethodName, setting);
        }
        
        public void Error(string UserMessage, Exception Ex)
        {
            string setting = ConfigurationManager.AppSettings["logerror"];
            Write(UserMessage, "Error", Ex, "", setting);
        }

        public void Error(string UserMessage, Exception Ex, string MethodName)
        {
            string setting = ConfigurationManager.AppSettings["logerror"];
            Write(UserMessage, "Error", Ex, MethodName, setting);
        }

        private void Write(string UserMessage, string Level, Exception Exception, string MethodName, string Setting)
        {
            if (!String.IsNullOrWhiteSpace(this.connectionString) && !String.IsNullOrEmpty(Setting))
            {
                try
                {
                    string usrmsg = "";
                    string exmsg = "";
                    string trace = "";
                    string methodname = "";

                    if (!String.IsNullOrEmpty(UserMessage)) usrmsg = UserMessage;
                    if (Exception != null && Level=="Error")
                    {
                        if (!String.IsNullOrEmpty(Exception.Message)) exmsg = Exception.Message;
                        if (!String.IsNullOrEmpty(Exception.StackTrace)) trace = Exception.StackTrace;
                    }
                    if (!String.IsNullOrEmpty(MethodName)) methodname = MethodName;

                    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(this.connectionString);
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(@"INSERT INTO [OPSI_APPLICATION_LOG]
                                                                                                   (
                                                                                                    [Date], 
                                                                                                    [Level], 
                                                                                                    [MethodName], 
                                                                                                    [Message], 
                                                                                                    [StackTrace], 
                                                                                                    [UserData]
                                                                                                    )
                                                                                             VALUES
                                                                                                   (getdate()
                                                                                                   ,@Level 
                                                                                                   ,@methodname 
                                                                                                   ,@exmsg 
                                                                                                   ,@trace  
                                                                                                   ,@usrmsg)", conn);

                    cmd.Parameters.Add("@Level", SqlDbType.VarChar, 10).Value = (object)Level ?? DBNull.Value;
                    cmd.Parameters.Add("@methodname", SqlDbType.VarChar, 100).Value = (object)methodname ?? DBNull.Value;
                    cmd.Parameters.Add("@exmsg", SqlDbType.NVarChar).Value = (object)exmsg ?? DBNull.Value;
                    cmd.Parameters.Add("@trace", SqlDbType.NVarChar).Value = (object)trace ?? DBNull.Value;
                    cmd.Parameters.Add("@usrmsg", SqlDbType.NVarChar).Value = (object)usrmsg ?? DBNull.Value;

                    conn.Open();
                    cmd.ExecuteScalar();
                    conn.Close();
                }
                catch 
                { }
            }
        }

    }
}