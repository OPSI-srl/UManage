using UManage_Repository.Entities;
using UManage_Repository.Repos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace UManage_Repository.Repos
{
    internal class UsersRepository : BaseRepository, IUsersRepository
    {

        #region "Log"

        public static OPSI_Logger.ErrorLog log = new OPSI_Logger.ErrorLog();

        #endregion

        #region Data Retrieval

        public List<UserEntity> ListUsers
           (
                int portalId,
                int ResultsPerPage,
                int CurrentPage,
                string searchKey,
                string roles,
                bool deleted,
                bool unauth,
                string orderby,
                string orderclause
            )
        {

            //log.Debug("Function called", "OPSI_Reports.Repository.CategoryRepository.ListAll");
            List<UserEntity> items = new List<UserEntity>();

            try
            {

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand("OPSI_UManage_UsersSearch", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@PortalID", SqlDbType.Int).Value = portalId;
                    command.Parameters.Add("@NumberItems", SqlDbType.Int).Value = ResultsPerPage;
                    command.Parameters.Add("@SelectPage", SqlDbType.Int).Value = CurrentPage;
                    command.Parameters.Add("@Filter_Key", SqlDbType.NVarChar, 100).Value = searchKey;
                    command.Parameters.Add("@Filter_Roles", SqlDbType.NVarChar, 100).Value = roles;
                    command.Parameters.Add("@Filter_Deleted", SqlDbType.Bit).Value = deleted;
                    command.Parameters.Add("@Filter_Unauth", SqlDbType.Bit).Value = unauth;
                    command.Parameters.Add("@OrderBy", SqlDbType.NVarChar,50).Value = orderby;
                    command.Parameters.Add("@OrderClause", SqlDbType.NVarChar, 50).Value = orderclause;

                    using (SqlDataReader dr = command.ExecuteReader(CommandBehavior.SingleResult))
                    {

                        while (dr.Read())
                        {
                            var item = BuildEntity(dr);
                            items.Add(item);
                        }


                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

            return items;

        }

        #endregion

        #region Private Helpers

        private UserEntity BuildEntity(IDataRecord dr)
        {

            UserEntity entity = new UserEntity();

            entity.UserID = dr.Get<int>("UserID");
            entity.DisplayName = dr.GetString("DisplayName");
            entity.Email = dr.GetString("Email");
            entity.Telephone = dr.GetString("Telephone");
            entity.FirstName = dr.GetString("FirstName");
            entity.LastName = dr.GetString("LastName");
            entity.Username = dr.GetString("Username");
            entity.DisplayName = dr.GetString("DisplayName");
            //entity.Password = "";
            entity.CreatedOnDate = dr.Get<DateTime>("CreatedOnDate");
            entity.LastLoginDate = dr.Get<DateTime>("LastLoginDate");
            entity.IsDeleted = dr.Get<Boolean>("IsDeleted");
            entity.Authorised = dr.Get<Boolean>("Authorised");
            entity.Profile_Picture_FileID = dr.GetString("Profile_Picture_FileID");
            entity.TotalRows = dr.Get<int>("TotalRows");
            entity.Pages = dr.Get<int>("Pages");

            return entity;

        }

        #endregion

        public virtual void Dispose()
        {
            this.Dispose();
        }

    }

    internal interface IUsersRepository : IDisposable
    {
        List<UserEntity> ListUsers(
                int portalId,
                int ResultsPerPage,
                int CurrentPage,
                string searchKey,
                string roles,
                bool deleted,
                bool unauth,
                string orderby,
                string orderclause
            );

    }

}

