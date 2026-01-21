using ProjectAPI.Entity;


namespace ProjectAPI.Interfaces
{
    public interface IAuth
    {
        AuthEntity? GetUserByUsername(string username);
    }
}
