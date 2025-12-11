using Microsoft.AspNetCore.Http;

namespace FileStorageService.Dtos;

public class UploadFileRequest
{
    public string StudentName { get; set; } = null!;
    public string AssignmentName { get; set; } = null!;
    public IFormFile File { get; set; } = null!;
}
