using ProjectAPI.Entity;
using ProjectAPI.ViewModel;

namespace ProjectAPI.Interfaces
{
    public interface IUserReg
    {
       Task<long> SaveUser(UserRegEntity userRegEntity);
        Task<List<UserRegEntity>> GetAllUsers();
        Task<UserRegOperationsEntity> GetUsersById(int userId);
    }
}
