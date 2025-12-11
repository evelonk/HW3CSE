using System.Net.Http.Headers;

namespace ApiGateway.Services;

public class FileStorageProxyService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public FileStorageProxyService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    private string BaseUrl => _config["Services:FileStorage"]!;

    public async Task<HttpResponseMessage> ForwardUploadAsync(HttpRequest request)
    {
        var url = $"{BaseUrl}/api/files";

        var multipart = new MultipartFormDataContent();

        foreach (var field in request.Form)
        {
            multipart.Add(new StringContent(field.Value), field.Key);
        }

        var file = request.Form.Files.First();

        var streamContent = new StreamContent(file.OpenReadStream());
        streamContent.Headers.ContentType =
            new MediaTypeHeaderValue(file.ContentType);

        multipart.Add(streamContent, "File", file.FileName);

        return await _http.PostAsync(url, multipart);
    }


    public async Task<HttpResponseMessage> GetFilesAsync()
        => await _http.GetAsync($"{BaseUrl}/api/files");

    public async Task<HttpResponseMessage> DownloadFileAsync(Guid id)
        => await _http.GetAsync($"{BaseUrl}/api/files/{id}/download");

    public async Task<HttpResponseMessage> GetByHashAsync(string hash)
        => await _http.GetAsync($"{BaseUrl}/api/files/by-hash/{hash}");

}
