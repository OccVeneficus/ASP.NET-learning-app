using Microsoft.EntityFrameworkCore;

namespace TestApp.Model;

public class TreeDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public TreeDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlite database
        options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
    }

    public DbSet<TreeNode> TreeNodes { get; set; }
}