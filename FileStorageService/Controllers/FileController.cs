using FileStorageService.Data;
using FileStorageService.Dtos;
using FileStorageService.Models;
using FileStorageService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace FileStorageService.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly FileStorageDbContext _db;
    private readonly S3StorageService _s3;
    private readonly HttpClient _http;

    public FilesController(FileStorageDbContext db, S3StorageService s3, HttpClient http)
    {
        _db = db;
        _s3 = s3;
        _http = http;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] UploadFileRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("Файл не передан");

        using var ms = new MemoryStream();
        await request.File.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var hash = HashService.ComputeSha256(bytes);

        var fileId = Guid.NewGuid();
        var s3Key = $"{fileId}_{request.File.FileName}";

        await _s3.UploadAsync(s3Key, bytes);

        var entity = new StoredFile
        {
            Id = fileId,
            OriginalName = request.File.FileName,
            S3Bucket = "files",
            S3Key = s3Key,
            ContentHash = hash,
            UploadedAt = DateTime.UtcNow,
            StudentName = request.StudentName,
            AssignmentName = request.AssignmentName
        };

        _db.Files.Add(entity);
        await _db.SaveChangesAsync();

        await _http.PostAsync(
    $"http://fileanalysisservice:8080/api/analysis/check/{entity.Id}",
    null
);


        return Ok(new
        {
            fileId = entity.Id,
            hash = entity.ContentHash
        });

    }
    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var file = await _db.Files.FirstOrDefaultAsync(x => x.Id == id);
        if (file == null)
            return NotFound("Файл не найден");

        var bytes = await _s3.DownloadAsync(file.S3Key);

        return File(bytes, "application/octet-stream", file.OriginalName);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var files = await _db.Files
            .OrderByDescending(x => x.UploadedAt)
            .ToListAsync();

        return Ok(files);
    }
}
