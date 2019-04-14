using School.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace School.UI.Pages.SchoolClasses
{
    public class DeleteModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public DeleteModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SchoolClass SchoolClass { get; set; }
        public string ConcurrencyErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, bool? concurrencyError)
        {
            SchoolClass = await _context.SchoolClasses
                .Include(d => d.ClassTeacher)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SchoolClassID == id);

            if (SchoolClass == null)
            {
                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ConcurrencyErrorMessage = "The record you attempted to delete "
                  + "was modified by another user after you selected delete. "
                  + "The delete operation was canceled and the current values in the "
                  + "database have been displayed. If you still want to delete this "
                  + "record, click the Delete button again.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                if (!await _context.SchoolClasses.AnyAsync(
                    m => m.SchoolClassID == id)) return RedirectToPage("./Index");
                
                _context.SchoolClasses.Remove(SchoolClass);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToPage("./Delete",
                    new { concurrencyError = true, id = id });
            }
        }
    }
}