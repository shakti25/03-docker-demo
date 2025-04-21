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
    public class Edit : PageModel
    {
        private readonly ILogger<Edit> _logger;
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public TodoItem TodoItem { get; set; }

        public Edit(ILogger<Edit> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TodoItem = await _context.TodoItems.FindAsync(id);

            if(TodoItem is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is invalid for {Title}", TodoItem.Title);
                return Page();
            }

            // Procesar manualmente el valor del checkbox si es necesario
            if (Request.Form.TryGetValue("TodoItem.IsCompleted", out var isCompletedValues))
            {
                TodoItem.IsCompleted = isCompletedValues.Last() == "true";
            }

            var todoInDb = await _context.TodoItems.FindAsync(TodoItem.Id);
            if(todoInDb is null)
            {
                _logger.LogError("Todo Item with Id {Id} was not found", TodoItem.Id);
                return NotFound(); // Retornar 404 si no se encuentra
            }

            todoInDb.Title = TodoItem.Title;
            todoInDb.IsCompleted = TodoItem.IsCompleted;

            await _context.SaveChangesAsync();

            _logger.LogInformation("TodoItem with id '{Id}' was updated", TodoItem.Id);

            return RedirectToPage("Index");
        }
    }
}