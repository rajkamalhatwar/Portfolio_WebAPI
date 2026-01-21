using Azure.Storage.Blobs;
using Portfolio_APIs.ServiceInterfaces;
using Portfolio_APIs.ViewModel;

namespace Portfolio_APIs.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly string _containerName;
        private readonly string _baseUrl;

        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlob:ConnectionString"];
            _containerName = configuration["AzureBlob:ContainerName"];
            _baseUrl = configuration["AzureBlob:BaseUrl"]; 

            _containerClient = new BlobContainerClient(connectionString, _containerName);
            _containerClient.CreateIfNotExists(); 

            
        } 

        public async Task<VMblobStorage> UploadAsync(IFormFile file, string folderName)
        {
            // students/profile/xxxx.jpg
            var timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var random = Random.Shared.Next(1000, 9999);
            var relativePath =
                 $"{folderName}/{timeStamp}_{random}{Path.GetExtension(file.FileName)}";

            var blobClient = _containerClient.GetBlobClient(relativePath);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return new VMblobStorage
            {
                RelativePath = relativePath,
                FullUrl = $"{_baseUrl}/{_containerName}/{relativePath}"
            };
        }
    }
}
