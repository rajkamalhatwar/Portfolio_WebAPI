using DataAccess;
using Portfolio_APIs.Entity;
using Portfolio_APIs.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.Repository
{
    public class ProjectRepo : IProjectRepo
    {
        SqlHelper objSqlHelper = new SqlHelper();

        public async Task<int> DeleteProjectById(int projectId, int userId)
        {
            object ret;
            try
            {
                // 🔥 Prepare SQL Parameters (Match SP exactly)
                SqlParameter[] objParams = new SqlParameter[3];

                objParams[0] = new SqlParameter("@ProjectId", SqlDbType.Int)
                {
                    Value = projectId
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
                    "[dbo].[sp_DeleteProjectById]",
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

        public async Task<List<ProjectEntity?>> GetProjectByIdAsync(int? projectId, int userId)
        {
            List<ProjectEntity> result = new();

            SqlParameter[] objParams = new SqlParameter[]
            {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                new SqlParameter("@ProjectId", SqlDbType.Int)
                {
                    Value = (object?)projectId ?? DBNull.Value
                }
            };

            DataSet ds = objSqlHelper.ExecuteDataSetSP(
                "[dbo].[sp_GetProjectByUserId]",
                objParams
            );

            DataTable? dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;

            if (dt == null || dt.Rows.Count == 0)
                return result;

            // 🔥 LINQ GROUPING (Master + Child)
            result = dt.AsEnumerable()
                .GroupBy(r => r.Field<int>("ProjectId"))
                .Select(g =>
                {
                    var first = g.First();

                    return new ProjectEntity
                    {
                        Id = first.Field<int>("ProjectId"),
                        ProjectName = first.Field<string>("ProjectName"),
                        ShortDescription = first.Field<string>("ShortDescription"),
                        GitHubLink = first.Field<string>("GitHubLink"),
                        LiveLink = first.Field<string>("LiveLink"),
                        DemoLink = first.Field<string>("DemoLink"),
                        TechStack = first.Field<string>("TechStack"), 
                        SequenceNo = first.Field<int>("SequenceNo"),
                        UserId = first.Field<int>("UserId"), 

                        Features = g
                            .Where(x => x["Feature"] != DBNull.Value)
                            .Select(x => new ProjectFeaturesEntity
                            {
                                Feature = x.Field<string>("Feature")
                            })
                            .ToList()
                    };
                })
                .OrderBy(x => x.SequenceNo)
                .ToList();

            return result;
        }

        public async Task<int> SubmitProjectInfoAsync(ProjectEntity projectEntity)
        {
            object ret;

            try
            {
                // 🔥 Create DataTable for Feature TVP
                DataTable featureTable = new DataTable();
                featureTable.Columns.Add("Feature", typeof(string));

                if (projectEntity.Features != null && projectEntity.Features.Count > 0)
                {
                    foreach (var item in projectEntity.Features)
                    {
                        featureTable.Rows.Add(item.Feature);
                    }
                }

                // 🔥 SQL Parameters
                SqlParameter[] objParams = new SqlParameter[11];

                objParams[0] = new SqlParameter("@Id", SqlDbType.Int)
                { Value = projectEntity.Id };

                objParams[1] = new SqlParameter("@ProjectName", SqlDbType.NVarChar, 300)
                { Value = (object?)projectEntity.ProjectName ?? DBNull.Value };

                objParams[2] = new SqlParameter("@ShortDescription", SqlDbType.NVarChar, 1000)
                { Value = (object?)projectEntity.ShortDescription ?? DBNull.Value };

                objParams[3] = new SqlParameter("@GitHubLink", SqlDbType.NVarChar, 500)
                { Value = (object?)projectEntity.GitHubLink ?? DBNull.Value };

                objParams[4] = new SqlParameter("@LiveLink", SqlDbType.NVarChar, 500)
                { Value = (object?)projectEntity.LiveLink ?? DBNull.Value };

                objParams[5] = new SqlParameter("@DemoLink", SqlDbType.NVarChar, 500)
                { Value = (object?)projectEntity.DemoLink ?? DBNull.Value };

                objParams[6] = new SqlParameter("@SequenceNo", SqlDbType.Int)
                { Value = projectEntity.SequenceNo };

                objParams[7] = new SqlParameter("@TechStack", SqlDbType.NVarChar, 500)
                { Value = (object?)projectEntity.TechStack ?? DBNull.Value };

                objParams[8] = new SqlParameter("@UserId", SqlDbType.Int)
                { Value = projectEntity.UserId };

                // 🔥 Feature TVP
                objParams[9] = new SqlParameter("@Features", SqlDbType.Structured)
                {
                    TypeName = "dbo.ProjectFeatureTVP",
                    Value = featureTable
                };

                objParams[10] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Execute SP
                ret = objSqlHelper.ExecuteNonQuerySP(
                    "[dbo].[sp_InsertOrUpdateProject]",
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

 
    }
}
