namespace BlogApi.Models;

public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Excerpt { get; set; }
    public string? CoverImage { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsPublished { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public List<Comment> Comments { get; set; } = new();
}
