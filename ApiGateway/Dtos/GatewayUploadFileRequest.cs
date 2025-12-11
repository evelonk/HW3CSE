using Microsoft.AspNetCore.Http;

namespace ApiGateway.Dtos;

public class GatewayUploadFileRequest
{
    public string StudentName { get; set; } = string.Empty;
    public string AssignmentName { get; set; } = string.Empty;
    public IFormFile File { get; set; } = null!;
}
