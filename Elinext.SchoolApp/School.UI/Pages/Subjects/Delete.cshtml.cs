using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using School.UI.Models;

namespace School.UI.Pages.Subjects
{
    public class DeleteModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public DeleteModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subject Subject { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subject = await _context.Subjects
                     .AsNoTracking()
                     .Include(c => c.SchoolClass)
                     .FirstOrDefaultAsync(m => m.SubjectID == id);

            if (Subject == null)
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

            Subject = await _context.Subjects
                       .AsNoTracking()
                       .FirstOrDefaultAsync(m => m.SubjectID == id);

            if (Subject != null)
            {
                _context.Subjects.Remove(Subject);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
