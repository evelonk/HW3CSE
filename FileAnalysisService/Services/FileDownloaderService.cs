using static System.Net.WebRequestMethods;
using System.Text.Json;

namespace FileAnalysisService.Services;

public class FileDownloaderService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public FileDownloaderService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<Dictionary<string, object>>> GetAllFilesAsync()
    {
        var response = await _httpClient.GetAsync("http://filestorageservice:8080/api/files");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Failed to get files list");

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json)!;
    }



    public async Task<byte[]> DownloadFileAsync(Guid fileId)
    {
        var baseUrl = _configuration["FileStorage:BaseUrl"];
        var url = $"{baseUrl}/api/files/{fileId}/download";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Failed to download file from FileStorageService");

        return await response.Content.ReadAsByteArrayAsync();
    }
}
