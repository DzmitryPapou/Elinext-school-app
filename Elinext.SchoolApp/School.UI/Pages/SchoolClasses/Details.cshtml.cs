using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using School.UI.Models;

namespace School.UI.Pages.SchoolClasses
{
    public class DetailsModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public DetailsModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        public SchoolClass SchoolClass { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SchoolClass = await _context.SchoolClasses
                .Include(d => d.ClassTeacher).SingleOrDefaultAsync(m => m.SchoolClassID == id);

            if (SchoolClass == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
