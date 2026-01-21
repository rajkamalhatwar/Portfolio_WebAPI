using Portfolio_APIs.Entity;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.Interfaces
{
    public interface IProjectRepo
    {
        Task<int> SubmitProjectInfoAsync(ProjectEntity projectEntity);
        Task<List<ProjectEntity?>> GetProjectByIdAsync(int? projectId, int userId);
        Task<int> DeleteProjectById(int projectId, int userId);
    }
}
