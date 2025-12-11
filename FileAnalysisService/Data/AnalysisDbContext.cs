using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FileAnalysisService.Data;

public class AnalysisDbContext : DbContext
{
    public AnalysisDbContext(DbContextOptions<AnalysisDbContext> options)
    : base(options)
    {
    }

    public DbSet<AnalysisReport> Reports => Set<AnalysisReport>();
}
