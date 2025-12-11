using FileStorageService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileStorageService.Data;

public class FileStorageDbContext : DbContext
{
    public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options)
        : base(options)
    {
    }

    public DbSet<StoredFile> Files => Set<StoredFile>();
}
