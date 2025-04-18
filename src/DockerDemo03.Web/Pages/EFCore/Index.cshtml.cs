using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DockerDemo03.Web.Pages.EFCore;

public class IndexModel : PageModel {
    private readonly ILogger<IndexModel> _logger;
    public IndexModel(ILogger<IndexModel> logger) {
        _logger = logger;
    }

    public void OnGet() {
        // This method is called on GET requests to the page.
        // You can add any initialization logic here if needed.
        _logger.LogInformation("Index page loaded at {Time}", DateTime.UtcNow);
    }
}