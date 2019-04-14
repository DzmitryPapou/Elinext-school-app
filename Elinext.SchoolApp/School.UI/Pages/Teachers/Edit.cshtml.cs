using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.UI.Models;

namespace School.UI.Pages.Teachers
{
    public class EditModel : TeacherSubjectsPageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public EditModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Teacher Teacher { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Teacher = await _context.Teachers
                .Include(i => i.PositionAssignments)
                .Include(i => i.SubjectAssignments).ThenInclude(i => i.Subject)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Teacher == null)
            {
                return NotFound();
            }
            PopulateAssignedSubjectData(_context, Teacher);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCourses)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var instructorToUpdate = await _context.Teachers
                .Include(i => i.PositionAssignments)
                .Include(i => i.SubjectAssignments)
                    .ThenInclude(i => i.Subject)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (await TryUpdateModelAsync<Teacher>(
                instructorToUpdate,
                "Teacher",
                i => i.FirstMidName, i => i.LastName,
                i => i.PositionAssignments))
            {
                if (String.IsNullOrWhiteSpace(
                    instructorToUpdate.PositionAssignments?.Position))
                {
                    instructorToUpdate.PositionAssignments = null;
                }
                UpdateTeacherSubjects(_context, selectedCourses, instructorToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            UpdateTeacherSubjects(_context, selectedCourses, instructorToUpdate);
            PopulateAssignedSubjectData(_context, instructorToUpdate);
            return Page();
        }
    }
}
