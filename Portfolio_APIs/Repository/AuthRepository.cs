using DataAccess;
using ProjectAPI.Entity;
using ProjectAPI.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace ProjectAPI.Repository
{
    public class AuthRepository : IAuth
    {
        SqlHelper objSqlHelper = new SqlHelper();

        public AuthEntity? GetUserByUsername(string username)
        {
            try
            {
                SqlParameter[] objParams = null;
                objParams = new SqlParameter[1];
                DataSet ds = new DataSet(); 
                objParams[0] = new SqlParameter("@P_Username", username);
                ds = objSqlHelper.ExecuteDataSetSP("GetUserDetail", objParams);

                if (ds == null)
                {
                    return null;
                }

                DataRow row = ds.Tables[0].Rows[0];

                // Map dataset row to AuthEntity
                AuthEntity user = new AuthEntity
                {
                    UserId = row["UserId"] != DBNull.Value ? Convert.ToInt32(row["UserId"]) : 0,
                    Username = row["Email"] != DBNull.Value ? Convert.ToString(row["Email"]) : string.Empty, 
                    PasswordHash = row["Password"] != DBNull.Value ? Convert.ToString(row["Password"]) : string.Empty 
                }; 
                return user; 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
 
}
