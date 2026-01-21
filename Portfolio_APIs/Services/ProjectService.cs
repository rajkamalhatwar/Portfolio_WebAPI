using Portfolio_APIs.Entity;
using Portfolio_APIs.Interfaces;
using Portfolio_APIs.Repository;
using Portfolio_APIs.ServiceInterfaces;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo _IProjectRepo;
        public ProjectService(IProjectRepo iProjectRepo)
        {
            _IProjectRepo = iProjectRepo;
        }

        public async Task<int> DeleteProjectById(int projectId, int userId)
        {
            int result = await _IProjectRepo.DeleteProjectById(projectId, userId);
            return await Task.FromResult(result);
        }

        public async Task<List<VMProject?>> GetProjectByIdAsync(int? projectId, int userId)
        {
            var projects = await _IProjectRepo.GetProjectByIdAsync(projectId, userId);

            if (projects == null || projects.Count == 0)
                return new List<VMProject>();


            var projectVMs = projects.Select(e => new VMProject
            {
                Id = e.Id,
                ProjectName = e.ProjectName,
                ShortDescription = e.ShortDescription, 
                GitHubLink = e.GitHubLink,
                LiveLink = e.LiveLink, 
                DemoLink = e.DemoLink,
                TechStack = e.TechStack,  
                SequenceNo = e.SequenceNo,
                UserId = e.UserId, 
                Features = e.Features.Select(a => new VMProjectFeatures
                {
                    Feature = a.Feature
                }).ToList()

            }).ToList();

            return projectVMs;
        }

        public async Task<int> SubmitProjectInfoAsync(VMProject vMProject)
        {
            ProjectEntity projectEntity = new ProjectEntity
            {
                Id = vMProject.Id,
                ProjectName = vMProject.ProjectName,
                ShortDescription = vMProject.ShortDescription,
                GitHubLink = vMProject.GitHubLink,
                LiveLink = vMProject.LiveLink,
                DemoLink = vMProject.DemoLink,
                SequenceNo = vMProject.SequenceNo,
                TechStack = vMProject.TechStack,
                UserId = vMProject.UserId,
                Features = vMProject.Features?.Select(s => new ProjectFeaturesEntity
                {
                    Feature = s.Feature

                }).ToList() ?? new List<ProjectFeaturesEntity>()
          
 
            };

            int result = await _IProjectRepo.SubmitProjectInfoAsync(projectEntity);
            return result;
        }
    }
}
