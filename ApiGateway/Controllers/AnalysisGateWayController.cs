using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiGateway.Controllers;

[ApiController]
[Route("works")]
public class AnalysisGatewayController : ControllerBase
{
    private readonly FileAnalysisProxyService _proxy;

    public AnalysisGatewayController(FileAnalysisProxyService proxy)
    {
        _proxy = proxy;
    }

    [HttpGet("{assignment}/reports")]
    public async Task<IActionResult> GetReportsForWork(string assignment)
    {
        try
        {
            var response = await _proxy.GetByAssignmentAsync(assignment);

            var content = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, content);
        }
        catch
        {
            return StatusCode(
                (int)HttpStatusCode.ServiceUnavailable,
                "Сервис аналитики временно недоступен"
            );
        }
    }

    [HttpGet("files/{fileId}/reports")]
    public async Task<IActionResult> GetReportsByFile(Guid fileId)
    {
        try
        {
            var response = await _proxy.GetByFileAsync(fileId);

            var content = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, content);
        }
        catch
        {
            return StatusCode(
                (int)HttpStatusCode.ServiceUnavailable,
                "Сервис аналитики временно недоступен"
            );
        }
    }
}
