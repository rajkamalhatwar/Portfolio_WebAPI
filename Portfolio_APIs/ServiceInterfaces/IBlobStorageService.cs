using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.ServiceInterfaces
{
    public interface IBlobStorageService
    {
        public Task<VMblobStorage> UploadAsync(IFormFile file, string folderName);
    }
}
