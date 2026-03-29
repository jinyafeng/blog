using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Data;

public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options)
    {
    }

    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 配置Tags属性为JSON存储
        modelBuilder.Entity<BlogPost>()
            .Property(b => b.Tags)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            );

        // 种子数据
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "技术", Slug = "tech", Description = "技术相关文章" },
            new Category { Id = 2, Name = "生活", Slug = "life", Description = "生活随笔" },
            new Category { Id = 3, Name = "笔记", Slug = "notes", Description = "学习笔记" }
        );
    }
}
