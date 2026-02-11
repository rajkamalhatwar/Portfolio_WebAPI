using Portfolio_APIs.Entity;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.ServiceInterfaces
{
    public interface ICreativeWorksService
    {
        Task<int> SubmitWorkCategaryInfoAsync(VMWorkCatogory vMWorkCatogory);
        Task<List<VMWorkCatogory?>> GetWorkCategaryByIdAsync(int? workCategoryId, int userId);
        Task<int> DeleteWorkCategaryById(int workCategoryId, int userId);

        Task<int> SubmitCreativeWorksInfoAsync(VMCreativeWork vMCreativeWork);
        Task<List<VMCreativeWork?>> GetCreativeWork(int? workCategoryId, int userId);
        Task<int> DeleteCreativeWorkById(int creativeWorkId, int userId);
    }
}
