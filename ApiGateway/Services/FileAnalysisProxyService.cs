namespace ApiGateway.Services;

public class FileAnalysisProxyService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public FileAnalysisProxyService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    private string BaseUrl => _config["Services:FileAnalysis"]!;

    public async Task<HttpResponseMessage> GetByFileAsync(Guid fileId)
    => await _http.GetAsync($"{BaseUrl}/api/analysis/by-file/{fileId}");

    public async Task<HttpResponseMessage> GetByAssignmentAsync(string assignment)
        => await _http.GetAsync($"{BaseUrl}/api/analysis/by-assignment/{assignment}");

}
