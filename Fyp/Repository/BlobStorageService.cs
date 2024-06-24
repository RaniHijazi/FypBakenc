using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly DataContext _context;

    public BlobStorageService(IConfiguration configuration, DataContext context)
    {
        string connectionString = configuration.GetConnectionString("AzureBlobStorage");
        _blobServiceClient = new BlobServiceClient(connectionString);
        _containerName = configuration["AzureBlobStorage:ContainerName"];
        _context = context;
      
    }

    public async Task<string> UploadImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("Invalid image file");

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        string blobName = Guid.NewGuid().ToString() + "_" + image.FileName;
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        using (var stream = image.OpenReadStream())
        {
            await blobClient.UploadAsync(stream);
        }

        return blobClient.Uri.ToString();
    }

    public async Task SaveUserUrlAsync(int userId, IFormFile image)
    {
        string imageUrl = await UploadImageAsync(image);

        
        var user = await _context.users.FindAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found");
        }

        user.ProfilePath = imageUrl;
        await _context.SaveChangesAsync();
    }

    public async Task<string> UploadStoryAsync(IFormFile story)
    {
        if (story == null || story.Length == 0)
            throw new ArgumentException("Invalid story file");

        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        string blobName = Guid.NewGuid().ToString() + "_" + story.FileName;
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        using (var stream = story.OpenReadStream())
        {
            await blobClient.UploadAsync(stream);
        }

        return blobClient.Uri.ToString();
    }

    


}
