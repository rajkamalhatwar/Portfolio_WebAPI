using Portfolio_APIs.Entity;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.ServiceInterfaces
{
    public interface IProjectService
    { 
        Task<int> SubmitProjectInfoAsync(VMProject vMProject);
        Task<List<VMProject?>> GetProjectByIdAsync(int? projectId, int userId);
        Task<int> DeleteProjectById(int projectId, int userId);
    }
}
