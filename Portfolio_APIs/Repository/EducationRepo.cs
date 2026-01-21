using DataAccess;
using Portfolio_APIs.Entity;
using Portfolio_APIs.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace Portfolio_APIs.Repository
{
    public class EducationRepo : IEducationRepo
    {
        SqlHelper objSqlHelper = new SqlHelper();

        public async Task<int> DeleteEducationById(int educationId, int userId)
        {
            object ret;
            try
            {
                // 🔥 Prepare SQL Parameters (Match SP exactly)
                SqlParameter[] objParams = new SqlParameter[3];

                objParams[0] = new SqlParameter("@EducationId", SqlDbType.Int)
                {
                    Value = educationId
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
                    "[dbo].[sp_DeleteEducationById]",
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

        public async Task<int> DeleteSkillById(int skillId, int userId)
        {
            object ret;
            try
            {
                
                SqlParameter[] objParams = new SqlParameter[3];

                objParams[0] = new SqlParameter("@SkillId", SqlDbType.Int)
                {
                    Value = skillId
                };

                objParams[1] = new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Value = userId
                }; 
                
                objParams[2] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                }; 

                ret = objSqlHelper.ExecuteNonQuerySP("[dbo].[sp_DeleteSkillById]", objParams,true);

                return ret == null ? -99 : Convert.ToInt32(ret);
            }
            catch (Exception)
            {
                return -99;
            }
        }

        public async Task<List<EducationEntity?>> GetEducationByIdAsync(int? educationId, int userId)
        { 
            List<EducationEntity> educationEntityList = new List<EducationEntity>();  

            SqlParameter[] parameters =
            {
                new SqlParameter("@EducationId", SqlDbType.Int) { Value = educationId },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };

            using (DataSet ds = objSqlHelper.ExecuteDataSetSP("[dbo].[sp_GetEducationById]", parameters))
            {
                //if (ds.Tables[0].Rows.Count == 0)
                //    return null;

                if (ds.Tables.Count < 2 || ds.Tables[0].Rows.Count == 0)
                    return educationEntityList;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    educationEntityList.Add(new EducationEntity
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        DegreeName = row["DegreeName"].ToString()!,
                        BranchName = row["BranchName"] as string,
                        MarkType = row["MarkType"] as string,
                        Marks = row["Marks"] as decimal?,
                        AdmissionMonth = row["AdmissionMonth"] as string,
                        AdmissionYear = row["AdmissionYear"] as int?,
                        PassingMonth = row["PassingMonth"] as string,
                        PassingYear = row["PassingYear"] as int?,
                        CollegeName = row["CollegeName"] as string,
                        CollegeAddress = row["CollegeAddress"] as string,
                        UserId = Convert.ToInt32(row["UserId"]),
                        IsActive = Convert.ToBoolean(row["IsActive"]),
                        Skills = new List<SkillEntity>(), // 👈 important
                        SequenceNo = row["SequenceNo"] as int? ?? 0
                    });
                }

                //foreach (DataRow sRow in ds.Tables[1].Rows)
                //{
                //    int eduId = Convert.ToInt32(sRow["EducationId"]);

                //    var education = educationEntityList
                //        .FirstOrDefault(e => e.Id == eduId);

                //    if (education != null)
                //    {
                //        education.Skills.Add(new SkillEntity
                //        {
                //            Id = Convert.ToInt32(sRow["Id"]),
                //            SkillName = sRow["SkillName"].ToString()!,
                //            OutOf100 = sRow["OutOf100"] as int?,
                //            EducationId = eduId,
                //            UserId = Convert.ToInt32(sRow["UserId"])
                //        });
                //    }
                //}

                //DataRow row = ds.Tables[0].Rows[0];
                //educationEntity = new EducationEntity
                //{
                //    Id = Convert.ToInt32(row["Id"]),
                //    DegreeName = row["DegreeName"].ToString()!,
                //    BranchName = row["BranchName"] as string,
                //    MarkType = row["MarkType"] as string,
                //    Marks = row["Marks"] as decimal?,
                //    AdmissionMonth = row["AdmissionMonth"] as string,
                //    AdmissionYear = row["AdmissionYear"] as int?,
                //    PassingMonth = row["PassingMonth"] as string,
                //    PassingYear = row["PassingYear"] as int?,
                //    CollegeName = row["CollegeName"] as string,
                //    CollegeAddress = row["CollegeAddress"] as string,
                //    UserId = Convert.ToInt32(row["UserId"]),
                //    IsActive = Convert.ToBoolean(row["IsActive"])
                //};

                // Skills
                //foreach (DataRow sRow in ds.Tables[1].Rows)
                //{
                //    educationEntity.Skills.Add(new SkillEntity
                //    {
                //        Id = Convert.ToInt32(sRow["Id"]),
                //        SkillName = sRow["SkillName"].ToString()!,
                //        OutOf100 = sRow["OutOf100"] as int?,
                //        EducationId = Convert.ToInt32(sRow["EducationId"]),
                //        UserId = Convert.ToInt32(sRow["UserId"])
                //    });
                //}
            }

            return educationEntityList;
        }

        public async Task<List<SkillEntity?>> GetSkillsByIdAsync(int userId)
        {
            List<SkillEntity> skillEntityList = new List<SkillEntity>();

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };

            using (DataSet ds = objSqlHelper.ExecuteDataSetSP(
                "[dbo].[sp_GetSkillsByUserId]",
                parameters))
            {
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return skillEntityList;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    skillEntityList.Add(new SkillEntity
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        SkillName = row["SkillName"].ToString()!,
                        OutOf100 = row["OutOf100"] == DBNull.Value ? null : Convert.ToInt32(row["OutOf100"]),
                        SequenceNo = row["SequenceNo"] == DBNull.Value ? null : Convert.ToInt32(row["SequenceNo"]),
                        UserId = Convert.ToInt32(row["UserId"])
                    });
                }
            }

            return skillEntityList;
        }


        public async Task<int> SubmitEducationInfoAsync(EducationEntity educationEntity)
        {
            object ret;

            try
            {
                // 🔥 Create DataTable for Skill TVP
                DataTable skillTable = new DataTable();
                skillTable.Columns.Add("SkillName", typeof(string));
                skillTable.Columns.Add("OutOf100", typeof(int));

                if (educationEntity.Skills != null && educationEntity.Skills.Count > 0)
                {
                    foreach (var skill in educationEntity.Skills)
                    {
                        skillTable.Rows.Add(
                            skill.SkillName,
                            skill.OutOf100
                        );
                    }
                }

                // 🔥 Prepare SQL Parameters (Match SP exactly)
                SqlParameter[] objParams = new SqlParameter[16];

                objParams[0] = new SqlParameter("@Id", SqlDbType.Int){ Value = educationEntity.Id }; 
                objParams[1] = new SqlParameter("@DegreeName", SqlDbType.NVarChar, 200) { Value = educationEntity.DegreeName };  
                objParams[2] = new SqlParameter("@BranchName", SqlDbType.NVarChar, 200) { Value = (object?)educationEntity.BranchName ?? DBNull.Value }; 
                objParams[3] = new SqlParameter("@MarkType", SqlDbType.NVarChar, 100) { Value = (object?)educationEntity.MarkType ?? DBNull.Value };  
                objParams[4] = new SqlParameter("@Marks", SqlDbType.Decimal)
                {
                    Precision = 5,
                    Scale = 2,
                    Value = (object?)educationEntity.Marks ?? DBNull.Value
                }; 
                objParams[5] = new SqlParameter("@AdmissionMonth", SqlDbType.NVarChar, 100)
                { Value = (object?)educationEntity.AdmissionMonth ?? DBNull.Value };

                objParams[6] = new SqlParameter("@AdmissionYear", SqlDbType.Int)
                { Value = (object?)educationEntity.AdmissionYear ?? DBNull.Value };

                objParams[7] = new SqlParameter("@PassingMonth", SqlDbType.NVarChar, 100)
                { Value = (object?)educationEntity.PassingMonth ?? DBNull.Value };

                objParams[8] = new SqlParameter("@PassingYear", SqlDbType.Int)
                { Value = (object?)educationEntity.PassingYear ?? DBNull.Value };

                objParams[9] = new SqlParameter("@CollegeName", SqlDbType.NVarChar, 300)
                { Value = (object?)educationEntity.CollegeName ?? DBNull.Value };

                objParams[10] = new SqlParameter("@CollegeAddress", SqlDbType.NVarChar, 300)
                { Value = (object?)educationEntity.CollegeAddress ?? DBNull.Value };

                objParams[11] = new SqlParameter("@UserId", SqlDbType.Int)
                { Value = educationEntity.UserId };

                objParams[12] = new SqlParameter("@IsActive", SqlDbType.Bit)
                { Value = educationEntity.IsActive };

                // 🔥 TVP Parameter
                objParams[13] = new SqlParameter("@Skills", SqlDbType.Structured)
                {
                    TypeName = "dbo.SkillTVP",
                    Value = skillTable
                };
                objParams[14] = new SqlParameter("@SequenceNo", SqlDbType.Int)
                { Value = (object?)educationEntity.SequenceNo ?? DBNull.Value };

                // 🔥 Output Parameter
                objParams[15] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Execute SP
                ret = objSqlHelper.ExecuteNonQuerySP(
                    "[dbo].[sp_InsertOrUpdateEducationInfo]",
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

        public async Task<int> SubmitSkillInfo(SkillEntity skillEntity)
        {
            object ret;

            try
            {  
                SqlParameter[] objParams = new SqlParameter[6]; 
                objParams[0] = new SqlParameter("@Id", SqlDbType.Int) { Value = skillEntity.Id };
                objParams[1] = new SqlParameter("@SkillName", SqlDbType.NVarChar, 200) { Value = skillEntity.SkillName };
                objParams[2] = new SqlParameter("@OutOf100", SqlDbType.Int) { Value = skillEntity.OutOf100 };
                objParams[3] = new SqlParameter("@UserId", SqlDbType.Int) { Value = skillEntity.UserId };
                objParams[4] = new SqlParameter("@SequenceNo", SqlDbType.Int) { Value = skillEntity.SequenceNo };  
                objParams[5] = new SqlParameter("@Res", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                }; 
                 
                ret = objSqlHelper.ExecuteNonQuerySP("[dbo].[sp_InsertOrUpdateSkill]", objParams,true); 
                return ret == null ? -99 : Convert.ToInt32(ret);
            }
            catch (Exception)
            {
                return -99;
            }
        }
    }
}
