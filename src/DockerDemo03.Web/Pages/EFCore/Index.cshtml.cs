using DockerDemo03.Web.Data;
using DockerDemo03.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DockerDemo03.Web.Pages.EFCore;

public class IndexModel : PageModel {
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;

    public List<TodoItem> TodoItems { get; set; }

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context) {
        _logger = logger;
        _context = context;
    }

    public async Task OnGetAsync() {
        _logger.LogInformation("Index page loaded at {Time}", DateTime.UtcNow);

        TodoItems = await _context.TodoItems.ToListAsync();

        // TodoItems = new List<TodoItem> {
        //     new TodoItem { Id = 1, Title = "Learn ASP.NET Core", IsCompleted = false },
        //     new TodoItem { Id = 2, Title = "Build a web application", IsCompleted = true },
        //     new TodoItem { Id = 3, Title = "Deploy to Docker", IsCompleted = false }
        // };
    }
}