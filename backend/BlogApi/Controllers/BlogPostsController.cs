using BlogApi.Data;
using BlogApi.Models;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostsController : ControllerBase
{
    private readonly BlogContext _context;

    public BlogPostsController(BlogContext context)
    {
        _context = context;
    }

    // GET: api/BlogPosts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts(bool onlyPublished = true)
    {
        var posts = await _context.BlogPosts
            .Include(p => p.Category)
            .Include(p => p.Comments.Where(c => c.IsApproved))
            .Where(p => !onlyPublished || p.IsPublished)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return posts;
    }

    // GET: api/BlogPosts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BlogPost>> GetBlogPost(int id, bool renderMarkdown = false)
    {
        var blogPost = await _context.BlogPosts
            .Include(p => p.Category)
            .Include(p => p.Comments.Where(c => c.IsApproved))
            .FirstOrDefaultAsync(p => p.Id == id);

        if (blogPost == null)
        {
            return NotFound();
        }

        if (renderMarkdown)
        {
            blogPost.Content = Markdown.ToHtml(blogPost.Content);
        }

        return blogPost;
    }

    // GET: api/BlogPosts/slug/my-first-post
    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<BlogPost>> GetBlogPostBySlug(string slug, bool renderMarkdown = false)
    {
        var blogPost = await _context.BlogPosts
            .Include(p => p.Category)
            .Include(p => p.Comments.Where(c => c.IsApproved))
            .FirstOrDefaultAsync(p => p.Slug == slug);

        if (blogPost == null)
        {
            return NotFound();
        }

        if (renderMarkdown)
        {
            blogPost.Content = Markdown.ToHtml(blogPost.Content);
        }

        return blogPost;
    }

    // PUT: api/BlogPosts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBlogPost(int id, BlogPost blogPost)
    {
        if (id != blogPost.Id)
        {
            return BadRequest();
        }

        blogPost.UpdatedAt = DateTime.Now;
        _context.Entry(blogPost).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BlogPostExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/BlogPosts
    [HttpPost]
    public async Task<ActionResult<BlogPost>> PostBlogPost(BlogPost blogPost)
    {
        blogPost.CreatedAt = DateTime.Now;
        _context.BlogPosts.Add(blogPost);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBlogPost", new { id = blogPost.Id }, blogPost);
    }

    // DELETE: api/BlogPosts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlogPost(int id)
    {
        var blogPost = await _context.BlogPosts.FindAsync(id);
        if (blogPost == null)
        {
            return NotFound();
        }

        _context.BlogPosts.Remove(blogPost);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BlogPostExists(int id)
    {
        return _context.BlogPosts.Any(e => e.Id == id);
    }
}
