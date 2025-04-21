using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerDemo03.Web.Data;
using DockerDemo03.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DockerDemo03.Web.Pages.EFCore
{
    public class Create : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Create> _logger;

        public Create(ILogger<Create> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public TodoItem TodoItem { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                _logger.LogError("ModelState is invalid for {Title}", TodoItem.Title);
                return Page();
            }

            if (Request.Form.TryGetValue("TodoItem.IsCompleted", out var isCompletedValues))
            {
                TodoItem.IsCompleted = isCompletedValues.Last() == "true";
            }

            _context.TodoItems.Add(TodoItem);
            await _context.SaveChangesAsync();
            _logger.LogInformation("TodoItem with title '{Title}' was created", TodoItem.Title);
            return RedirectToPage("Index");
        }
    }
}