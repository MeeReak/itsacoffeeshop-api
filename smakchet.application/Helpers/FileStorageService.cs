using Amazon.S3;
using Amazon.S3.Model;
using smakchet.application.DTOs.IFileStorageService;

namespace smakchet.application.Helpers;

public class FileStorageService(IAmazonS3 s3Client) : IFileStorageService
{
    private readonly string _bucketName = "itsacoffeeshop";

    private async Task EnsureBucketExistsAsync(string bucketName)
    {
        var buckets = await s3Client.ListBucketsAsync();
        if (!buckets.Buckets.Any(b => b.BucketName == bucketName))
        {
            await s3Client.PutBucketAsync(new PutBucketRequest { BucketName = bucketName });
        }
    }

    public async Task<string> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string folder,
        CancellationToken cancellationToken)
    {
        await EnsureBucketExistsAsync(_bucketName);

        var safeFileName = Path.GetFileName(fileName); 
        var uuidFileName = $"{Guid.NewGuid()}_{safeFileName}";
        var key = $"{folder}/{uuidFileName}";  

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = fileStream,
            CannedACL = S3CannedACL.PublicRead
        };

        await s3Client.PutObjectAsync(request, cancellationToken);

        return $"/{_bucketName}/{key}"; 
    }

    public async Task<Stream> GetFileAsync(string fileName, string folder, CancellationToken cancellationToken)
    {
        var bucketName = folder.ToLower();
        var response = await s3Client.GetObjectAsync(bucketName, fileName, cancellationToken);
        return response.ResponseStream;
    }

    public async Task DeleteFileAsync(string fileName, string folder, CancellationToken cancellationToken)
    {
        var key = $"{folder}/{Path.GetFileName(fileName)}";

        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await s3Client.DeleteObjectAsync(deleteRequest, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetAllFilesAsync(string folder, CancellationToken cancellationToken)
    {
        var fileNames = new List<string>();

        var listRequest = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = folder + "/" 
        };

        ListObjectsV2Response response;
        do
        {
            response = await s3Client.ListObjectsV2Async(listRequest, cancellationToken);
            fileNames.AddRange(response.S3Objects.Select(o => o.Key));
            listRequest.ContinuationToken = response.NextContinuationToken;
        } while ((bool)response.IsTruncated!);

        return fileNames;
    }
}