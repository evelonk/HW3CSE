using Amazon.S3;
using Amazon.S3.Model;

namespace FileStorageService.Services;

public class S3StorageService
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucket;

    public S3StorageService(IConfiguration configuration)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = configuration["S3:ServiceUrl"],
            ForcePathStyle = true
        };

        _s3 = new AmazonS3Client(
            configuration["S3:AccessKey"],
            configuration["S3:SecretKey"],
            config
        );

        _bucket = configuration["S3:Bucket"]!;

        EnsureBucketExistsAsync().GetAwaiter().GetResult();
    }

    private async Task EnsureBucketExistsAsync()
    {
        try
        {
            var response = await _s3.ListBucketsAsync();

            var buckets = response.Buckets ?? new List<S3Bucket>();

            if (!buckets.Any(b => b.BucketName == _bucket))
            {
                await _s3.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = _bucket,
                    UseClientRegion = true
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"S3 bucket check failed: {ex.Message}");
            throw;
        }
    }


    public async Task UploadAsync(string key, byte[] data)
    {
        using var stream = new MemoryStream(data);

        var request = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = key,
            InputStream = stream
        };

        await _s3.PutObjectAsync(request);
    }

    public async Task<byte[]> DownloadAsync(string key)
    {
        var response = await _s3.GetObjectAsync(_bucket, key);

        using var ms = new MemoryStream();
        await response.ResponseStream.CopyToAsync(ms);
        return ms.ToArray();
    }
}
