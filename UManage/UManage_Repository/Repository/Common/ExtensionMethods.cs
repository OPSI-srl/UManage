using System;
using System.Data;
using System.Data.Common;


namespace UManage_Repository.Repos
{
    public static class ExtensionMethods
    {

        #region IDataRecord

        public static T? GetNullable<T>(this IDataRecord dr, string field) where T : struct
        {
            return (T?)(dr[field] == DBNull.Value ? null : Convert.ChangeType(dr[field], typeof(T)));
        }

        public static T Get<T>(this IDataRecord dr, string field)
        {
            return (T)(dr[field] == DBNull.Value ? null : Convert.ChangeType(dr[field], typeof(T)));
        }

        public static string GetString(this IDataRecord dr, string field)
        {
            return dr[field] == DBNull.Value ? null : Convert.ToString(dr[field]);
        }

        public static string GetTrimmedString(this IDataRecord dr, string field)
        {
            string _value = GetString(dr, field);
            return _value != null ? _value.Trim() : null;
        }

        #endregion

        #region ADODB.Recordset

        public static T? GetNullable<T>(this ADODB.Recordset dr, string field) where T : struct
        {
            return (T?)(dr.Fields[field].Value == DBNull.Value ? null : Convert.ChangeType(dr.Fields[field].Value, typeof(T)));
        }

        public static T Get<T>(this ADODB.Recordset dr, string field)
        {
            return (T)(dr.Fields[field].Value == DBNull.Value ? null : Convert.ChangeType(dr.Fields[field].Value, typeof(T)));
        }

        public static string GetString(this ADODB.Recordset dr, string field)
        {
            return dr.Fields[field].Value == DBNull.Value ? null : Convert.ToString(dr.Fields[field].Value).Trim();
        }

        public static string GetTrimmedString(this ADODB.Recordset dr, string field)
        {
            string _value = GetString(dr, field);
            return _value != null ? _value.Trim() : null;
        }

        #endregion

        #region DbCommand

        public static T? ExecuteScalarNullable<T>(this DbCommand cmd) where T : struct
        {
            object obj = cmd.ExecuteScalar();
            return (T?)((obj == null || obj == DBNull.Value) ? null : Convert.ChangeType(obj, typeof(T)));
        }

        public static T ExecuteScalar<T>(this DbCommand cmd)
        {
            object obj = cmd.ExecuteScalar();
            return (T)((obj == null || obj == DBNull.Value) ? null : Convert.ChangeType(obj, typeof(T)));
        }

        public static string ExecuteScalarString(this DbCommand cmd)
        {
            object obj = cmd.ExecuteScalar();
            return (obj == null || obj == DBNull.Value) ? null : Convert.ToString(obj);
        }

        public static string ExecuteScalarTrimmedString(this DbCommand cmd)
        {
            string _value = cmd.ExecuteScalarString();
            return _value != null ? _value.Trim() : null;
        }

        #endregion

    }
}
