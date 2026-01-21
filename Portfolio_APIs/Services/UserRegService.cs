using ProjectAPI.Entity;
using ProjectAPI.Interfaces;
using ProjectAPI.Repository;
using ProjectAPI.ServiceInterfaces;
using ProjectAPI.ViewModel;
using System.Threading.Tasks;

namespace ProjectAPI.Services
{
    public class UserRegService : IUserRegService
    {
        private readonly IUserReg  _IUserReg;
        public UserRegService(IUserReg  iUserReg)
        {
            _IUserReg = iUserReg;
        }

        public async Task<List<VMUserReg>> GetAllUsers()
        {


            var users = await _IUserReg.GetAllUsers();

            // Map Entity to ViewModel
            var userVMs = users.Select(u => new VMUserReg
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                MobileNo = u.MobileNo,
                Designations = u.Designations,
                HeroLine = u.HeroLine,
                ShortAbout = u.ShortAbout,
                LongAbout = u.LongAbout,
                Address = u.Address,
                City = u.City,
                State = u.State,
                Country = u.Country,
                TwitterLink = u.TwitterLink,
                LinkedInLink = u.LinkedInLink,
                GitHubLink = u.GitHubLink,
                InstagramLink = u.InstagramLink,
                BehanceLink = u.BehanceLink,
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate,
                PhotoUrl = u.PhotoUrl
            }).ToList();

            return userVMs;
        }

        public async Task<VMUserRegOperations> GetUsersById(int userId)
        { 
            var userRegOperations = await _IUserReg.GetUsersById(userId);
            
            var vmUserRegOperations = new VMUserRegOperations
            {
                GetUsersById = userRegOperations.GetUsersById?
                    .Select(user => new VMUserReg
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Email = user.Email,
                        MobileNo = user.MobileNo,
                        Designations = user.Designations,
                        HeroLine = user.HeroLine,
                        ShortAbout = user.ShortAbout,
                        LongAbout = user.LongAbout,
                        Address = user.Address,
                        City = user.City,
                        State = user.State,
                        Country = user.Country,
                        TwitterLink = user.TwitterLink,
                        LinkedInLink = user.LinkedInLink,
                        GitHubLink = user.GitHubLink,
                        InstagramLink = user.InstagramLink,
                        BehanceLink = user.BehanceLink,
                        IsActive = user.IsActive,
                        CreatedDate = user.CreatedDate,
                        PhotoUrl = user.PhotoUrl
                    }).ToList()

            }; 
            return vmUserRegOperations;
            

        }

        public async Task<long> SaveUser(VMUserReg vMUserReg)
        {
            UserRegEntity userRegEntity = new UserRegEntity
            {
                UserId = vMUserReg.UserId,
                UserName = vMUserReg.UserName,
                Email = vMUserReg.Email,
                MobileNo = vMUserReg.MobileNo,
                Password = vMUserReg.Password,
                Designations = vMUserReg.Designations,
                HeroLine = vMUserReg.HeroLine,
                ShortAbout = vMUserReg.ShortAbout,
                LongAbout = vMUserReg.LongAbout,
                Address = vMUserReg.Address,
                City = vMUserReg.City,
                State = vMUserReg.State,
                Country = vMUserReg.Country,
                TwitterLink = vMUserReg.TwitterLink,
                LinkedInLink = vMUserReg.LinkedInLink,
                GitHubLink = vMUserReg.GitHubLink,
                InstagramLink = vMUserReg.InstagramLink,
                BehanceLink = vMUserReg.BehanceLink, 
                IsActive = vMUserReg.IsActive
            };

            // Call the repository method
            long result = await _IUserReg.SaveUser(userRegEntity);

            return result;
        }
    }
}
