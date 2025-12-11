using System;

namespace FileStorageService.Models;

public class StoredFile
{
    public Guid Id { get; set; }

    public string OriginalName { get; set; } = null!;

    public string S3Bucket { get; set; } = null!;
    public string S3Key { get; set; } = null!;

    public string ContentHash { get; set; } = null!;
    public DateTime UploadedAt { get; set; }

    public string StudentName { get; set; } = null!;
    public string AssignmentName { get; set; } = null!;
}
