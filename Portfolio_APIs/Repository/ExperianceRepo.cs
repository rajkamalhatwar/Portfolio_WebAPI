using DataAccess;
using Portfolio_APIs.Entity;
using Portfolio_APIs.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace Portfolio_APIs.Repository
{
    public class ExperianceRepo : IExperianceRepo
    {
        SqlHelper objSqlHelper = new SqlHelper();

        public async Task<int> DeleteExperianceById(int experienceId, int userId)
        {
            object ret;
            try
            {
                // 🔥 Prepare SQL Parameters (Match SP exactly)
                SqlParameter[] objParams = new SqlParameter[3];

                objParams[0] = new SqlParameter("@ExperianceId", SqlDbType.Int)
                {
                    Value = experienceId
                };

                objParams[1] = new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Value = userId
                };

                // 🔥 Output Parameter
                objParams[2] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Execute SP
                ret = objSqlHelper.ExecuteNonQuerySP(
                    "[dbo].[sp_DeleteExperianceById]",
                    objParams,
                    true
                );

                return ret == null ? -99 : Convert.ToInt32(ret);
            }
            catch (Exception)
            {
                return -99;
            }
        }

        public async Task<List<ExperianceEntity?>> GetExperianceByIdAsync(int? experienceId, int userId)
        {
            List<ExperianceEntity> result = new();

            SqlParameter[] objParams = new SqlParameter[]
            {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                new SqlParameter("@ExperienceId", SqlDbType.Int)
                {
                    Value = (object?)experienceId ?? DBNull.Value
                }
            };

            DataSet ds = objSqlHelper.ExecuteDataSetSP(
                "[dbo].[sp_GetExperienceByUserId]",
                objParams
            );

            DataTable? dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;

            if (dt == null || dt.Rows.Count == 0)
                return result;

            // 🔥 LINQ GROUPING (Master + Child)
            result = dt.AsEnumerable()
                .GroupBy(r => r.Field<int>("ExperienceId"))
                .Select(g =>
                {
                    var first = g.First();

                    return new ExperianceEntity
                    {
                        Id = first.Field<int>("ExperienceId"),
                        CompanyName = first.Field<string>("CompanyName"),
                        Designation = first.Field<string>("Designation"),
                        JoiningMonth = first.Field<string>("JoiningMonth"),
                        JoiningYear = first.Field<int?>("JoiningYear"),
                        ReleaseMonth = first.Field<string>("ReleaseMonth"),
                        ReleaseYear = first.Field<int?>("ReleaseYear"),
                        Present = first.Field<bool>("Present"),
                        City = first.Field<string>("City"),
                        State = first.Field<string>("State"),
                        Country = first.Field<string>("Country"),
                        CompanyAddress = first.Field<string>("CompanyAddress"),
                        SequenceNo = first.Field<int>("SequenceNo"),
                        UserId = first.Field<int>("UserId"),
                        IsActive = first.Field<bool>("IsActive"),

                        Achievements = g
                            .Where(x => x["Achievement"] != DBNull.Value)
                            .Select(x => new ExperienceAchievementEntity
                            {
                                Achievement = x.Field<string>("Achievement")
                            })
                            .ToList()
                    };
                })
                .OrderBy(x => x.SequenceNo)
                .ToList();

            return result;
        }

        public async Task<int> SubmitExperianceInfoAsync(ExperianceEntity experianceEntity)
        {
            object ret;

            try
            {
                // 🔥 Create DataTable for Achievement TVP
                DataTable achievementTable = new DataTable();
                achievementTable.Columns.Add("Achievement", typeof(string));

                if (experianceEntity.Achievements != null && experianceEntity.Achievements.Count > 0)
                {
                    foreach (var item in experianceEntity.Achievements)
                    {
                        achievementTable.Rows.Add(item.Achievement);
                    }
                }

                // 🔥 SQL Parameters
                SqlParameter[] objParams = new SqlParameter[17];

                objParams[0] = new SqlParameter("@Id", SqlDbType.Int)
                { Value = experianceEntity.Id };

                objParams[1] = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 300)
                { Value = (object?)experianceEntity.CompanyName ?? DBNull.Value };

                objParams[2] = new SqlParameter("@Designation", SqlDbType.NVarChar, 200)
                { Value = (object?)experianceEntity.Designation ?? DBNull.Value };

                objParams[3] = new SqlParameter("@JoiningMonth", SqlDbType.NVarChar, 50)
                { Value = (object?)experianceEntity.JoiningMonth ?? DBNull.Value };

                objParams[4] = new SqlParameter("@JoiningYear", SqlDbType.Int)
                { Value = (object?)experianceEntity.JoiningYear ?? DBNull.Value };

                objParams[5] = new SqlParameter("@ReleaseMonth", SqlDbType.NVarChar, 50)
                {
                    Value = experianceEntity.Present
                        ? DBNull.Value
                        : (object?)experianceEntity.ReleaseMonth ?? DBNull.Value
                };

                objParams[6] = new SqlParameter("@ReleaseYear", SqlDbType.Int)
                {
                    Value = experianceEntity.Present
                        ? DBNull.Value
                        : (object?)experianceEntity.ReleaseYear ?? DBNull.Value
                };

                objParams[7] = new SqlParameter("@Present", SqlDbType.Bit)
                { Value = experianceEntity.Present };

                objParams[8] = new SqlParameter("@City", SqlDbType.NVarChar, 100)
                { Value = (object?)experianceEntity.City ?? DBNull.Value };

                objParams[9] = new SqlParameter("@State", SqlDbType.NVarChar, 100)
                { Value = (object?)experianceEntity.State ?? DBNull.Value };

                objParams[10] = new SqlParameter("@Country", SqlDbType.NVarChar, 100)
                { Value = (object?)experianceEntity.Country ?? DBNull.Value };

                objParams[11] = new SqlParameter("@CompanyAddress", SqlDbType.NVarChar, 500)
                { Value = (object?)experianceEntity.CompanyAddress ?? DBNull.Value };

                objParams[12] = new SqlParameter("@SequenceNo", SqlDbType.Int)
                { Value = experianceEntity.SequenceNo };

                objParams[13] = new SqlParameter("@UserId", SqlDbType.Int)
                { Value = experianceEntity.UserId };

                objParams[14] = new SqlParameter("@IsActive", SqlDbType.Bit)
                { Value = experianceEntity.IsActive };

                // 🔥 Achievement TVP
                objParams[15] = new SqlParameter("@Achievements", SqlDbType.Structured)
                {
                    TypeName = "dbo.ExperienceAchievementTVP",
                    Value = achievementTable
                };
                objParams[16] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Execute SP
                ret = objSqlHelper.ExecuteNonQuerySP("[dbo].[sp_InsertOrUpdateExperienceInfo]", objParams,true);

                return ret == null ? -99 : Convert.ToInt32(ret);
            }
            catch (Exception)
            {
                return -99;
            }
        }
    }
}
