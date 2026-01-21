using Portfolio_APIs.Entity;
using Portfolio_APIs.Interfaces;
using Portfolio_APIs.Repository;
using Portfolio_APIs.ServiceInterfaces;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.Services
{
    public class ExperianceService : IExperianceService
    {
        private readonly IExperianceRepo _IExperianceRepo;
        public ExperianceService(IExperianceRepo iExperianceRepo) 
        { 
            _IExperianceRepo = iExperianceRepo;
        }

        public async Task<int> DeleteExperianceById(int experienceId, int userId)
        {
            int result = await _IExperianceRepo.DeleteExperianceById(experienceId, userId);
            return await Task.FromResult(result);
        }

        public async Task<List<VMExperiance?>> GetExperianceByIdAsync(int? experienceId, int userId)
        {
            var experiences = await _IExperianceRepo.GetExperianceByIdAsync(experienceId, userId);

            if (experiences == null || experiences.Count == 0)
                return new List<VMExperiance>();

             
            var experienceVMs = experiences.Select(e => new VMExperiance
            {
                Id = e.Id,
                CompanyName = e.CompanyName,
                Designation = e.Designation,

                JoiningMonth = e.JoiningMonth,
                JoiningYear = e.JoiningYear,

                ReleaseMonth = e.ReleaseMonth,
                ReleaseYear = e.ReleaseYear,

                Present = e.Present,

                City = e.City,
                State = e.State,
                Country = e.Country,

                CompanyAddress = e.CompanyAddress,

                SequenceNo = e.SequenceNo,
                UserId = e.UserId,
                IsActive = e.IsActive,

                Achievements = e.Achievements.Select(a => new VMExperienceAchievement
                {
                    Achievement = a.Achievement
                }).ToList()

            }).ToList();

            return experienceVMs;
        }

        public async Task<int> SubmitExperianceInfoAsync(VMExperiance vMExperiance)
        {
             ExperianceEntity experianceEntity = new ExperianceEntity
             {
                 Id = vMExperiance.Id,
                 CompanyName = vMExperiance.CompanyName,
                 Designation = vMExperiance.Designation,
                 JoiningMonth = vMExperiance.JoiningMonth,
                 JoiningYear = vMExperiance.JoiningYear,
                 ReleaseMonth = vMExperiance.ReleaseMonth,
                 ReleaseYear = vMExperiance.ReleaseYear,
                 Present = vMExperiance.Present,
                 City = vMExperiance.City,
                 State = vMExperiance.State,
                 Country = vMExperiance.Country,
                 CompanyAddress = vMExperiance.CompanyAddress,
                 UserId = vMExperiance.UserId,
                 SequenceNo = vMExperiance.SequenceNo,
                 IsActive = vMExperiance.IsActive,
                 Achievements = vMExperiance.Achievements?.Select(s => new ExperienceAchievementEntity
                 {
                     Achievement = s.Achievement 

                 }).ToList() ?? new List<ExperienceAchievementEntity>()
             };

             int result = await _IExperianceRepo.SubmitExperianceInfoAsync(experianceEntity); 
             return result;
        }
    }
}
