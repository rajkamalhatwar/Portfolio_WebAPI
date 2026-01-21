using Portfolio_APIs.Entity;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.ServiceInterfaces
{
    public interface IEducationService
    {
        Task<int> SubmitEducationInfoAsync(VMEducation vMEducation);
        Task<List<VMEducation?>> GetEducationByIdAsync(int? educationId, int userId);
        Task<List<VMSkill?>> GetSkillsByIdAsync(int userId);
        Task<int> DeleteEducationById(int educationId, int userId);
        Task<int> SubmitSkillInfo(VMSkill vMSkill);
        Task<int> DeleteSkillById(int skillId, int userId);
    }
}
