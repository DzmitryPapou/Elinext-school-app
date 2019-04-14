using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using School.UI.Data;
using School.UI.Models;

namespace School.UI.Pages.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public DetailsModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

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
    }
}
