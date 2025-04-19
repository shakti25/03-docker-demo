using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerDemo03.Web.Data;
using DockerDemo03.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DockerDemo03.Web.Pages.EFCore
{
    public class Delete : PageModel
    {
        private readonly ILogger<Delete> _logger;
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public TodoItem TodoItem { get; set; }

        public Delete(ILogger<Delete> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGet(int? id)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoInDb = await _context.TodoItems.FindAsync(TodoItem.Id);
            if(todoInDb is null)
            {
                _logger.LogError("Todo Item with Id {Id} was not found", TodoItem.Id);
                return NotFound(); // Retornar 404 si no se encuentra
            }

            try
            {
                _context.TodoItems.Remove(todoInDb);

                await _context.SaveChangesAsync();

                _logger.LogInformation("TodoItem with id '{Id}' was deleted", TodoItem.Id);

                return RedirectToPage("Index");
            } catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);

                return RedirectToAction("Delete", new { id = todoInDb.Id });
            }
        }
    }
}