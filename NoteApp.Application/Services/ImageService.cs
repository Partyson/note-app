using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NoteApp.Application.Interfaces;

namespace NoteApp.Application.Services;

public class ImageService : IImageService
{
    private readonly IAmazonS3 s3Client;
    private readonly string bucketName;

    public ImageService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        this.s3Client = s3Client;
        bucketName = configuration["YcStorage:BucketName"]
                     ?? throw new ArgumentNullException("BucketName is not configured.");
    }

    public async Task<string> UploadImageAsync(IFormFile file, Guid folderId)
    {
        var fileExtension = Path.GetExtension(file.FileName);

        var objectName = $"folderImages/{folderId}{fileExtension}";
        await using var stream = file.OpenReadStream();
        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = objectName,
            InputStream = stream,
            ContentType = file.ContentType,
            CannedACL = S3CannedACL.PublicRead
        };

        await s3Client.PutObjectAsync(putRequest);
        return $"https://storage.yandexcloud.net/{bucketName}/{objectName}";
    }
}