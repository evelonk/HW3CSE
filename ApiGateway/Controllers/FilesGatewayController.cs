using ApiGateway.Dtos;
using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiGateway.Controllers;

[ApiController]
[Route("works")]
public class FilesGatewayController : ControllerBase
{
    private readonly FileStorageProxyService _proxy;

    public FilesGatewayController(FileStorageProxyService proxy)
    {
        _proxy = proxy;
    }

    [HttpPost]
    public async Task<IActionResult> UploadWork([FromForm] GatewayUploadFileRequest request)
    {
        try
        {
            var response = await _proxy.ForwardUploadAsync(Request);

            var data = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";

            return File(data, contentType);
        }
        catch
        {
            return StatusCode(
                (int)HttpStatusCode.ServiceUnavailable,
                "Сервис хранения файлов временно недоступен"
            );
        }
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        try
        {
            var response = await _proxy.DownloadFileAsync(id);
            var bytes = await response.Content.ReadAsByteArrayAsync();

            return File(bytes, "application/octet-stream");
        }
        catch
        {
            return StatusCode(
                (int)HttpStatusCode.ServiceUnavailable,
                "Сервис хранения файлов временно недоступен"
            );
        }
    }
}
