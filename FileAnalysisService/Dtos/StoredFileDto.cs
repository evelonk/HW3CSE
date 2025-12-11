public class StoredFileDto
{
    public Guid Id { get; set; }
    public string OriginalName { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string AssignmentName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
