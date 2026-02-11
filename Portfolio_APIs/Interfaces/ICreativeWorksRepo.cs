using Portfolio_APIs.Entity;

namespace Portfolio_APIs.Interfaces
{
    public interface ICreativeWorksRepo
    {
        Task<int> SubmitWorkCategaryInfoAsync(WorkCatogoryEntity workCatogoryEntity);
        Task<List<WorkCatogoryEntity?>> GetWorkCategaryByIdAsync(int? workCategoryId, int userId);
        Task<int> DeleteWorkCategaryById(int workCategoryId, int userId);  

        Task<int> SubmitCreativeWorksInfoAsync(CreativeWorksEntity creativeWorksEntity);
        Task<List<CreativeWorksEntity?>> GetCreativeWork(int? workCategoryId, int userId);
        Task<int> DeleteCreativeWorkById(int creativeWorkId, int userId);
    }
}
