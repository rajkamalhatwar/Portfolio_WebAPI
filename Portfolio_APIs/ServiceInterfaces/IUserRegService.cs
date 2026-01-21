using ProjectAPI.Entity;
using ProjectAPI.ViewModel;

namespace ProjectAPI.ServiceInterfaces
{
    public interface IUserRegService
    {
        Task<long> SaveUser(VMUserReg vmUserReg);
        Task<List<VMUserReg>> GetAllUsers();
        Task<VMUserRegOperations> GetUsersById(int userId);
    }
}
