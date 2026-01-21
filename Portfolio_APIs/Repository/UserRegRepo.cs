using ProjectAPI.Entity;
using ProjectAPI.ServiceInterfaces;
using System.Data.SqlClient;
using System.Data;
using DataAccess;
using ProjectAPI.Interfaces;
using ProjectAPI.ViewModel;

namespace ProjectAPI.Repository
{
    public class UserRegRepo : IUserReg
    {
        SqlHelper objSqlHelper = new SqlHelper();

        public async Task<List<UserRegEntity>> GetAllUsers()
        {
             
            List<UserRegEntity> userRegEntity = new List<UserRegEntity>();
            try
            {
                SqlParameter[] par = new SqlParameter[0];
                // No parameters needed
                DataSet ds = objSqlHelper.ExecuteDataSetSP("sp_GetAllUsers", par); // Assuming you create this SP

                if (ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    userRegEntity = Dal.Service_Providers.TableToList.ConvertDataTable<UserRegEntity>(ds.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                // Optional: log the exception
            }

            return userRegEntity;
        }

        public async Task<UserRegOperationsEntity> GetUsersById(int userId)
        {
            UserRegOperationsEntity userRegOperationsEntity = new UserRegOperationsEntity();
            try
            {
                SqlParameter[] par = new SqlParameter[1];
                par[0] = new SqlParameter("@UserId", userId);

                DataSet ds = objSqlHelper.ExecuteDataSetSP("sp_GetUserById", par);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    
                    userRegOperationsEntity.GetUsersById  = Dal.Service_Providers.TableToList.ConvertDataTable<UserRegEntity>(ds.Tables[0]);
                    // var list = Dal.Service_Providers.TableToList.ConvertDataTable<UserRegEntity>(ds.Tables[0]);
                    //  user = list.FirstOrDefault() ?? new UserRegEntity();
                }
            }
            catch (Exception ex)
            {
                // Optional: log the exception
            }

            return userRegOperationsEntity;
        }

        public async Task<long> SaveUser(UserRegEntity userRegEntity)
        {
            object ret; 
 
            try
            {
                // Prepare parameters matching your SP
                SqlParameter[] objParams = new SqlParameter[19]; // Adjust size for all SP params

                objParams[0] = new SqlParameter("@UserId", SqlDbType.Int) { Value = userRegEntity.UserId ?? 0 };
                objParams[1] = new SqlParameter("@UserName", SqlDbType.NVarChar, 300) { Value = (object?)userRegEntity.UserName ?? DBNull.Value };
                objParams[2] = new SqlParameter("@Email", SqlDbType.NVarChar, 300) { Value = (object?)userRegEntity.Email ?? DBNull.Value };
                objParams[3] = new SqlParameter("@MobileNo", SqlDbType.NVarChar, 15) { Value = (object?)userRegEntity.MobileNo ?? DBNull.Value };
                objParams[4] = new SqlParameter("@Password", SqlDbType.NVarChar, -1) { Value = (object?)userRegEntity.Password ?? DBNull.Value };
                objParams[5] = new SqlParameter("@Designations", SqlDbType.NVarChar, 150) { Value = (object?)userRegEntity.Designations ?? DBNull.Value };
                objParams[6] = new SqlParameter("@HeroLine", SqlDbType.NVarChar, 255) { Value = (object?)userRegEntity.HeroLine ?? DBNull.Value };
                objParams[7] = new SqlParameter("@ShortAbout", SqlDbType.NVarChar, 500) { Value = (object?)userRegEntity.ShortAbout ?? DBNull.Value };
                objParams[8] = new SqlParameter("@LongAbout", SqlDbType.NVarChar, -1) { Value = (object?)userRegEntity.LongAbout ?? DBNull.Value };
                objParams[9] = new SqlParameter("@Address", SqlDbType.NVarChar, 300) { Value = (object?)userRegEntity.Address ?? DBNull.Value };
                objParams[10] = new SqlParameter("@City", SqlDbType.NVarChar, 300) { Value = (object?)userRegEntity.City ?? DBNull.Value };
                objParams[11] = new SqlParameter("@State", SqlDbType.NVarChar, 300) { Value = (object?)userRegEntity.State ?? DBNull.Value };
                objParams[12] = new SqlParameter("@Country", SqlDbType.NVarChar, 300) { Value = (object?)userRegEntity.Country ?? DBNull.Value };
                objParams[13] = new SqlParameter("@TwitterLink", SqlDbType.NVarChar, 255) { Value = (object?)userRegEntity.TwitterLink ?? DBNull.Value };
                objParams[14] = new SqlParameter("@LinkedInLink", SqlDbType.NVarChar, 255) { Value = (object?)userRegEntity.LinkedInLink ?? DBNull.Value };
                objParams[15] = new SqlParameter("@GitHubLink", SqlDbType.NVarChar, 255) { Value = (object?)userRegEntity.GitHubLink ?? DBNull.Value };
                objParams[16] = new SqlParameter("@InstagramLink", SqlDbType.NVarChar, 255) { Value = (object?)userRegEntity.InstagramLink ?? DBNull.Value };
                objParams[17] = new SqlParameter("@BehanceLink", SqlDbType.NVarChar, 255) { Value = (object?)userRegEntity.BehanceLink ?? DBNull.Value };
                objParams[18] = new SqlParameter("@Res", SqlDbType.Int);
                objParams[18].Direction = ParameterDirection.Output;  

                ret = objSqlHelper.ExecuteNonQuerySP("[dbo].[sp_InsertOrUpdateUser]", objParams, true);

                // Return output value
                return ret != null ? Convert.ToInt64(ret) : -99;
            }
            catch (Exception ex)
            { 
                return -99;
            }
        }

 
    }
}
