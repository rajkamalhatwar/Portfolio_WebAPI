using ProjectAPI.ViewModel;

namespace ProjectAPI.ServiceInterfaces
{
    public interface IAuthService
    {
        LoginResponseViewModel Login(VMAuth model);
    }
}
