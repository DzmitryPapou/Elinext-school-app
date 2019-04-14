using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.UI.Data;
using School.UI.Models;

namespace School.UI.Pages.Subjects
{
    public class EditModel : SchoolClassNamePageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public EditModel(School.UI.Data.SchoolContext context)
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
                .Include(c => c.SchoolClass).FirstOrDefaultAsync(m => m.SubjectID == id);

            if (Subject == null)
            {
                return NotFound();
            }

            // Select current SchoolClassID.
            PopulateSubjectsDropDownList(_context, Subject.SchoolClassID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var courseToUpdate = await _context.Subjects.FindAsync(id);

            if (await TryUpdateModelAsync<Subject>(
                 courseToUpdate,
                 "subject",   // Prefix for form value.
                    c => c.SchoolClassID, c => c.Title))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Select SchoolClassID if TryUpdateModelAsync fails.
            PopulateSubjectsDropDownList(_context, courseToUpdate.SchoolClassID);
            return Page();
        }
    }
}