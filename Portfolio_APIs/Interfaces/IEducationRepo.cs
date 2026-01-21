using Portfolio_APIs.Entity;

namespace Portfolio_APIs.Interfaces
{
    public interface IEducationRepo
    {
        /// <summary>
        /// Insert or Update Education information along with Skills (TVP)
        /// Uses sp_InsertOrUpdateEducationInfo
        /// </summary>
        /// <param name="request">EducationInfoRequest entity</param>
        /// <returns>
        /// Result Code:
        /// 1 = Insert Success
        /// 2 = Update Success
        /// 3 = Already Exists
        /// 4 = Not Found
        /// -99 = Error
        /// </returns>
        Task<int> SubmitEducationInfoAsync(EducationEntity educationEntity);

        Task<List<EducationEntity?>> GetEducationByIdAsync(int? educationId, int userId);
        Task<List<SkillEntity?>> GetSkillsByIdAsync(int userId);
        Task<int> DeleteEducationById(int educationId, int userId);
        Task<int> SubmitSkillInfo(SkillEntity skillEntity); 
        Task<int> DeleteSkillById(int skillId, int userId);
    }
}
