using Microsoft.EntityFrameworkCore;

namespace TestApp.Model;

/// <summary>
/// Database context for tree view data accsess.
/// </summary>
public class TreeDbContext : DbContext
{
    /// <summary>
    /// App configuration.
    /// </summary>
    protected readonly IConfiguration _configuration;

    /// <summary>
    /// Set of tree nodes.
    /// </summary>
    public DbSet<TreeNode> TreeNodes { get; set; }

    /// <summary>
    /// Creates <see cref="TreeDbContext"/>.
    /// </summary>
    /// <param name="configuration">App configuration.</param>
    public TreeDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Populates database with initial values and saves it.
    /// </summary>
    public void SeedDb()
    {
        var rootNode1 = new TreeNode(1, -1, "Root 1");
        var rootNode2 = new TreeNode(2, -1, "Root 2");

        TreeNodes.AddRange(rootNode1, rootNode2);

        SaveChanges();

        var childNode1 = new TreeNode(3, 1,"Child 1");
        var childNode2 = new TreeNode(4, 1,"Child 2");
        var grandChild1 = new TreeNode(5, 3,"Grandchild 1");
        var grandChild2 = new TreeNode(6, 3,"Grandchild 2");

        TreeNodes.AddRange(childNode1, childNode2, grandChild1, grandChild2);

        SaveChanges();
    }

    /// <inheritdoc cref="DbContext.OnConfiguring"/>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(_configuration.GetConnectionString("WebApiDatabase"));
    }

    /// <inheritdoc cref="DbContext.OnModelCreating"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TreeNode>()
            .ToTable("TreeNodes");

        modelBuilder.Entity<TreeNode>()
            .Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }
}