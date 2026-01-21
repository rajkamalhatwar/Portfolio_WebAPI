using Portfolio_APIs.Entity;

namespace Portfolio_APIs.Interfaces
{
    public interface IExperianceRepo
    {
        Task<int> SubmitExperianceInfoAsync(ExperianceEntity experianceEntity);
        Task<List<ExperianceEntity?>> GetExperianceByIdAsync(int? experienceId, int userId); 
        Task<int> DeleteExperianceById(int experienceId, int userId);
    }
}
