using Portfolio_APIs.Entity;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.ServiceInterfaces
{
    public interface IExperianceService
    {
        Task<int> SubmitExperianceInfoAsync(VMExperiance vMExperiance);
        Task<List<VMExperiance?>> GetExperianceByIdAsync(int? experienceId, int userId);
        Task<int> DeleteExperianceById(int experienceId, int userId);
    }
}
