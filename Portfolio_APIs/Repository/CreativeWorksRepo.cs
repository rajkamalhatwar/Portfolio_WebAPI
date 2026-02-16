using Portfolio_APIs.Entity;
using Portfolio_APIs.Interfaces;
using System.Data.SqlClient;
using System.Data;
using DataAccess;

namespace Portfolio_APIs.Repository
{
    public class CreativeWorksRepo : ICreativeWorksRepo
    {
        SqlHelper objSqlHelper = new SqlHelper();
        public async Task<int> DeleteWorkCategaryById(int workCategoryId, int userId)
        {
            object ret;
            try
            {

                SqlParameter[] objParams = new SqlParameter[3];

                objParams[0] = new SqlParameter("@WorkCategaryId", SqlDbType.Int)
                {
                    Value = workCategoryId
                };

                objParams[1] = new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Value = userId
                };

                objParams[2] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                ret = objSqlHelper.ExecuteNonQuerySP("[dbo].[sp_DeleteWorkCategaryById]", objParams, true);

                return ret == null ? -99 : Convert.ToInt32(ret);
            }
            catch (Exception)
            {
                return -99;
            }
        } 
        public async Task<List<WorkCatogoryEntity?>> GetWorkCategaryByIdAsync(int? workCategoryId, int userId)
        {
            List<WorkCatogoryEntity> workCatogoryEntityList = new List<WorkCatogoryEntity>();

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };

            using (DataSet ds = objSqlHelper.ExecuteDataSetSP(
                "[dbo].[sp_GetWorkCategoryByUserId]",
                parameters))
            {
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return workCatogoryEntityList;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    workCatogoryEntityList.Add(new WorkCatogoryEntity
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        CategoryName = row["CategoryName"].ToString()!, 
                        SequenceNo = row["SequenceNo"] == DBNull.Value ? null : Convert.ToInt32(row["SequenceNo"]),
                        UserId = Convert.ToInt32(row["UserId"])
                    });
                }
            }

            return workCatogoryEntityList;
        }  
        public async Task<int> SubmitWorkCategaryInfoAsync(WorkCatogoryEntity workCatogoryEntity)
        {
            object ret;

            try
            {
                SqlParameter[] objParams = new SqlParameter[5];

                objParams[0] = new SqlParameter("@Id", SqlDbType.Int)
                {
                    Value = workCatogoryEntity.Id
                };

                objParams[1] = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 200)
                {
                    Value = (object?)workCatogoryEntity.CategoryName ?? DBNull.Value
                };

                objParams[2] = new SqlParameter("@SequenceNo", SqlDbType.Int)
                {
                    Value = (object?)workCatogoryEntity.SequenceNo ?? DBNull.Value
                };

                objParams[3] = new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Value = workCatogoryEntity.UserId
                };

                objParams[4] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                objSqlHelper.ExecuteNonQuerySP(
                    "[dbo].[sp_InsertOrUpdateWorkCategory]",
                    objParams,
                    true
                );

                return Convert.ToInt32(objParams[4].Value);
            }
            catch (Exception ex)
            {
                // log ex if needed
                return -99;
            }
        }

        public async Task<int> SubmitCreativeWorksInfoAsync(CreativeWorksEntity creativeWorksEntity)
        {
            object ret;

            try
            {
                SqlParameter[] objParams = new SqlParameter[9];

                objParams[0] = new SqlParameter("@Id", SqlDbType.Int)
                {
                    Value = creativeWorksEntity.Id
                };

                objParams[1] = new SqlParameter("@Title", SqlDbType.NVarChar, 200)
                {
                    Value = (object?)creativeWorksEntity.Title ?? DBNull.Value
                };

                objParams[2] = new SqlParameter("@Description", SqlDbType.NVarChar, 600)
                {
                    Value = (object?)creativeWorksEntity.Description ?? DBNull.Value
                };

                objParams[3] = new SqlParameter("@Tags", SqlDbType.NVarChar, 300)
                {
                    Value = (object?)creativeWorksEntity.Tags ?? DBNull.Value
                };

                objParams[4] = new SqlParameter("@ImageURL", SqlDbType.NVarChar, 500)
                {
                    Value = (object?)creativeWorksEntity.ImageURL ?? DBNull.Value
                };

                objParams[5] = new SqlParameter("@RelativeURL", SqlDbType.NVarChar, 200)
                {
                    Value = (object?)creativeWorksEntity.RelativeURL ?? DBNull.Value
                };


                objParams[6] = new SqlParameter("@WorkCategoryId", SqlDbType.Int)
                {
                    Value = (object?)creativeWorksEntity.WorkCategoryId ?? DBNull.Value
                };

                objParams[7] = new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Value = creativeWorksEntity.UserId
                };

                objParams[8] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                //objSqlHelper.ExecuteNonQuerySP(
                //    "[dbo].[sp_InsertOrUpdateCreativeWorks]",
                //    objParams,
                //    true
                //);

                //return Convert.ToInt32(objParams[8].Value);

                // Execute SP
                ret = objSqlHelper.ExecuteNonQuerySP(
                    "[dbo].[sp_InsertOrUpdateCreativeWorks]",
                    objParams,
                    true
                );

                return ret == null ? -99 : Convert.ToInt32(ret);
            }
            catch (Exception ex)
            {
                // log ex if needed
                return -99;
            }
        }

        public async Task<List<CreativeWorksEntity?>> GetCreativeWork(int? workCategoryId, int userId)
        {
            List<CreativeWorksEntity> creativeWorksEntityList = new List<CreativeWorksEntity>();

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                new SqlParameter("@WorkCategoryId", SqlDbType.Int) {Value = workCategoryId}
            };

            using (DataSet ds = objSqlHelper.ExecuteDataSetSP(
                "[dbo].[sp_GetCreativeWork]",
                parameters))
            {
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return creativeWorksEntityList;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    creativeWorksEntityList.Add(new CreativeWorksEntity
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Title = row["Title"].ToString()!,
                        Description = row["Description"].ToString()!,
                        Tags = row["Tags"].ToString()!,
                        ImageURL = row["ImageURL"].ToString()!,
                        WorkCategoryId = Convert.ToInt32(row["WorkCategoryId"]), 
                        UserId = Convert.ToInt32(row["UserId"]),
                        CategoryName = row["CategoryName"].ToString()!
                    });
                }
            }

            return creativeWorksEntityList;
        }

        public async Task<int> DeleteCreativeWorkById(int creativeWorkId, int userId)
        {
            object ret;
            try
            {

                SqlParameter[] objParams = new SqlParameter[3];

                objParams[0] = new SqlParameter("@CreativeWorkId", SqlDbType.Int)
                {
                    Value = creativeWorkId
                };

                objParams[1] = new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Value = userId
                };

                objParams[2] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                ret = objSqlHelper.ExecuteNonQuerySP("sp_DeleteCreativeWorkById", objParams, true);

                return ret == null ? -99 : Convert.ToInt32(ret);
            }
            catch (Exception)
            {
                return -99;
            }
        }
    }
}
