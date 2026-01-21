using Microsoft.VisualBasic;
using Portfolio_APIs.Entity;
using Portfolio_APIs.Interfaces;
using Portfolio_APIs.Repository;
using Portfolio_APIs.ServiceInterfaces;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.Services
{
    public class EducationService : IEducationService
    {
        private readonly IEducationRepo _IEducationRepo;
        public EducationService(IEducationRepo iEducationRepo)
        {
            _IEducationRepo = iEducationRepo;
        }

        public async Task<int> DeleteEducationById(int educationId, int userId)
        { 
            int result = await _IEducationRepo.DeleteEducationById(educationId, userId);
            return await Task.FromResult(result);
        }

        public async Task<int> DeleteSkillById(int skillId, int userId)
        {
            int result = await _IEducationRepo.DeleteSkillById(skillId, userId);
            return await Task.FromResult(result);
        }

        public async Task<List<VMEducation?>> GetEducationByIdAsync(int? educationId, int userId)
        {
            var educations = await _IEducationRepo.GetEducationByIdAsync(educationId, userId);

            if (educations == null || educations.Count == 0)
                return new List<VMEducation>();

            // Map Entity → ViewModel (same style as GetAllUsers)
            var educationVMs = educations.Select(e => new VMEducation
            {
                Id = e.Id,
                DegreeName = e.DegreeName,
                BranchName = e.BranchName,
                MarkType = e.MarkType,
                Marks = e.Marks,
                AdmissionMonth = e.AdmissionMonth,
                AdmissionYear = e.AdmissionYear,
                PassingMonth = e.PassingMonth,
                PassingYear = e.PassingYear,
                CollegeName = e.CollegeName,
                CollegeAddress = e.CollegeAddress,
                UserId = e.UserId,
                IsActive = e.IsActive,
                CreatedDate = e.CreatedDate,
                SequenceNo = e.SequenceNo,

                //Skills = e.Skills.Select(s => new VMSkill
                //{
                //    Id = s.Id,
                //    SkillName = s.SkillName,
                //    OutOf100 = s.OutOf100
                //}).ToList()

            }).ToList();

            return educationVMs;
        }

        public async Task<List<VMSkill?>> GetSkillsByIdAsync(int userId)
        {
            var skills = await _IEducationRepo.GetSkillsByIdAsync(userId);

            if (skills == null || skills.Count == 0)
                return new List<VMSkill>();

            var skillVMs = skills.Select(s => new VMSkill
            {
                Id = s.Id,
                SkillName = s.SkillName,
                OutOf100 = s.OutOf100,
                SequenceNo = s.SequenceNo,
                UserId = s.UserId
                
            }).ToList();

            return skillVMs;
        }

        public async Task<int> SubmitEducationInfoAsync(VMEducation vMEducation)
        {
            // 🔁 Map ViewModel → Entity (same style as SaveUser)
            EducationEntity entity = new EducationEntity
            {
                Id = vMEducation.Id,
                DegreeName = vMEducation.DegreeName,
                BranchName = vMEducation.BranchName,
                MarkType = vMEducation.MarkType,
                Marks = vMEducation.Marks,
                AdmissionMonth = vMEducation.AdmissionMonth,
                AdmissionYear = vMEducation.AdmissionYear,
                PassingMonth = vMEducation.PassingMonth,
                PassingYear = vMEducation.PassingYear,
                CollegeName = vMEducation.CollegeName,
                CollegeAddress = vMEducation.CollegeAddress,
                UserId = vMEducation.UserId,
                IsActive = vMEducation.IsActive,
                SequenceNo = vMEducation.SequenceNo,
                Skills = vMEducation.Skills?.Select(s => new SkillEntity
                {
                    SkillName = s.SkillName,
                    OutOf100 = s.OutOf100
                }).ToList() ?? new List<SkillEntity>()
            };

            // 🔥 Call repository
            int result = await _IEducationRepo.SubmitEducationInfoAsync(entity);

            return result;
        }

        public async Task<int> SubmitSkillInfo(VMSkill vMSkill)
        {
            SkillEntity skillEntity = new SkillEntity
            {
                Id = vMSkill.Id,
                SkillName = vMSkill.SkillName,
                OutOf100 = vMSkill.OutOf100,
                UserId = vMSkill.UserId,
                SequenceNo = vMSkill.SequenceNo
            };

            int result =  await _IEducationRepo.SubmitSkillInfo(skillEntity);
            return result;
        }
    }
}
