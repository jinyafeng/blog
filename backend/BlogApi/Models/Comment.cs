namespace BlogApi.Models;

public class Comment
{
    public int Id { get; set; }
    public int BlogPostId { get; set; }
    public BlogPost? BlogPost { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorEmail { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsApproved { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
