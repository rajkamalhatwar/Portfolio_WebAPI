using Portfolio_APIs.Entity;
using Portfolio_APIs.Interfaces;
using Portfolio_APIs.Repository;
using Portfolio_APIs.ServiceInterfaces;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.Services
{
    public class CreativeWorksService : ICreativeWorksService
    {
        private readonly ICreativeWorksRepo _ICreativeWorksRepo;
        private readonly IBlobStorageService _IBlobStorageService;
        public CreativeWorksService(ICreativeWorksRepo iCreativeWorksRepo, IBlobStorageService iBlobStorageService)
        {
            _ICreativeWorksRepo = iCreativeWorksRepo;
            _IBlobStorageService = iBlobStorageService;
        }

        public async Task<int> DeleteWorkCategaryById(int workCategoryId, int userId)
        {
            int result = await _ICreativeWorksRepo.DeleteWorkCategaryById(workCategoryId, userId);
            return await Task.FromResult(result);
        }
        public async Task<List<VMWorkCatogory?>> GetWorkCategaryByIdAsync(int? workCategoryId, int userId)
        {
            var workCatogories = await _ICreativeWorksRepo.GetWorkCategaryByIdAsync(workCategoryId, userId);

            if (workCatogories == null || workCatogories.Count == 0)
                return new List<VMWorkCatogory>();

            // Map Entity → ViewModel (same style as GetAllUsers)
            var workCatogoriesVMs = workCatogories.Select(e => new VMWorkCatogory
            {
                Id = e.Id,
                CategoryName = e.CategoryName, 
                UserId = e.UserId, 
                SequenceNo = e.SequenceNo, 

            }).ToList();

            return workCatogoriesVMs;
        } 
        public async Task<int> SubmitWorkCategaryInfoAsync(VMWorkCatogory vMWorkCatogory)
        {
            WorkCatogoryEntity workCatogoryEntity = new WorkCatogoryEntity
            {
                Id = vMWorkCatogory.Id,
                CategoryName = vMWorkCatogory.CategoryName, 
                UserId = vMWorkCatogory.UserId,
                SequenceNo = vMWorkCatogory.SequenceNo
            };

            int result = await _ICreativeWorksRepo.SubmitWorkCategaryInfoAsync(workCatogoryEntity);
            return result;
        }

        public async Task<int> SubmitCreativeWorksInfoAsync(VMCreativeWork vMCreativeWork)
        {

            string? relativePath = vMCreativeWork.RelativeURL;
            string? fullImageUrl = vMCreativeWork.ImageURL;

            // ✅ Upload only if new file is provided
            if (vMCreativeWork.FormFile != null)
            {
                var uploadResult = await _IBlobStorageService.UploadAsync(
                    vMCreativeWork.FormFile,
                    "creative-works-images"
                );

                relativePath = uploadResult.RelativePath; // store in DB
                fullImageUrl = uploadResult.FullUrl;       // return if needed
            }

            CreativeWorksEntity creativeWorksEntity = new CreativeWorksEntity
            {
                Id=vMCreativeWork.Id,
                Title = vMCreativeWork.Title,
                Description = vMCreativeWork.Description,
                Tags = vMCreativeWork.Tags,
                ImageURL = fullImageUrl,
                RelativeURL = relativePath,
                WorkCategoryId = vMCreativeWork.WorkCategoryId,
                UserId =vMCreativeWork.UserId,
            };
            int result = await _ICreativeWorksRepo.SubmitCreativeWorksInfoAsync(creativeWorksEntity);
            return result;
        }

        public async Task<List<VMCreativeWork?>> GetCreativeWork(int? workCategoryId, int userId)
        {
            var creativeWorks = await _ICreativeWorksRepo.GetCreativeWork(workCategoryId, userId);

            if (creativeWorks == null || creativeWorks.Count == 0)
                return new List<VMCreativeWork>();

            // Map Entity → ViewModel  
            var creativeWorksVMs = creativeWorks.Select(e => new VMCreativeWork
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Tags = e.Tags,
                ImageURL = e.ImageURL,
                WorkCategoryId = e.WorkCategoryId,
                UserId = e.UserId,
                CategoryName = e.CategoryName
            }).ToList();

            return creativeWorksVMs;
        }

        public async Task<int> DeleteCreativeWorkById(int creativeWorkId, int userId)
        {
            int result = await _ICreativeWorksRepo.DeleteCreativeWorkById(creativeWorkId, userId);
            return await Task.FromResult(result);
        }
    }
}
