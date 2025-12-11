using FileAnalysisService.Data;
using FileAnalysisService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Controllers;

[ApiController]
[Route("api/analysis")]
public class AnalysisController : ControllerBase
{
    private readonly AnalysisDbContext _db;
    private readonly FileDownloaderService _downloader;
    private readonly ByteComparisonService _comparator;

    public AnalysisController(
        AnalysisDbContext db,
        FileDownloaderService downloader,
        ByteComparisonService comparator)
    {
        _db = db;
        _downloader = downloader;
        _comparator = comparator;
    }

    

    [HttpPost("check/{fileId}")]
    public async Task<IActionResult> CheckFile(Guid fileId)
    {
        var allFiles = await _downloader.GetAllFilesAsync();

        var targetFile = allFiles.FirstOrDefault(f =>
            Guid.Parse(f["id"].ToString()!) == fileId);

        if (targetFile == null)
            return NotFound("Файл не найден в FileStorage");

        if (targetFile == null)
            return NotFound($"Файл {fileId} не найден в FileStorageService");

        var targetBytes = await _downloader.DownloadFileAsync(fileId);

        var targetAssignment = targetFile["assignmentName"].ToString();

        var candidates = allFiles
            .Where(f =>
                f["assignmentName"].ToString() == targetAssignment &&
                Guid.Parse(f["id"].ToString()!) != fileId)
            .ToList();


        double maxSimilarity = 0;
        Guid? matchedFileId = null;

        foreach (var file in candidates)
        {
            var candidateId = Guid.Parse(file["id"].ToString()!);

            var bytes = await _downloader.DownloadFileAsync(candidateId);
            var similarity = _comparator.Compare(targetBytes, bytes);

            if (similarity > maxSimilarity)
            {
                maxSimilarity = similarity;
                matchedFileId = candidateId;
            }
        }


        var isPlagiarism = maxSimilarity >= 70;

        var report = new AnalysisReport
        {
            Id = Guid.NewGuid(),
            CheckedFileId = fileId,
            MatchedFileId = matchedFileId,
            MaxSimilarityPercent = maxSimilarity,
            IsPlagiarism = isPlagiarism,
            CheckedAt = DateTime.UtcNow,
            StudentName = targetFile["studentName"].ToString(),
            AssignmentName = targetFile["assignmentName"].ToString()
        };

        _db.Reports.Add(report);
        await _db.SaveChangesAsync();

        return Ok(report);
    }

    [HttpGet("by-file/{fileId}")]
    public IActionResult GetByFile(Guid fileId)
    {
        var reports = _db.Reports
            .Where(r => r.CheckedFileId == fileId)
            .ToList();

        return Ok(reports);
    }

    [HttpGet("by-assignment/{assignment}")]
    public IActionResult GetByAssignment(string assignment)
    {
        var reports = _db.Reports
            .Where(r => r.AssignmentName == assignment)
            .ToList();

        return Ok(reports);
    }

}
