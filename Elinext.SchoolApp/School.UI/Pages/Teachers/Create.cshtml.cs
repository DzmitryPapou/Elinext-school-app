using School.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.UI.Pages.Teachers
{
    public class CreateModel : TeacherSubjectsPageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public CreateModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var teacher = new Teacher();
            teacher.SubjectAssignments = new List<SubjectAssignment>();

            // Provides an empty collection for the foreach loop
            // foreach (var subject in Model.AssignedCourseDataList)
            // in the Create Razor page.
            PopulateAssignedSubjectData(_context, teacher);

            Teacher = new Teacher
            {
                FirstMidName = "Rick",
                LastName = "Anderson"
            };
            return Page();
        }

        [BindProperty]
        public Teacher Teacher { get; set; }

        public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newInstructor = new Teacher();
            if (selectedCourses != null)
            {
                newInstructor.SubjectAssignments = new List<SubjectAssignment>();
                foreach (var subject in selectedCourses)
                {
                    var courseToAdd = new SubjectAssignment
                    {
                        SubjectID = int.Parse(subject)
                    };
                    newInstructor.SubjectAssignments.Add(courseToAdd);
                }
            }

            if (await TryUpdateModelAsync<Teacher>(
                newInstructor,
                "Teacher",
                i => i.FirstMidName, i => i.LastName,
                 i => i.PositionAssignments))
            {
                _context.Teachers.Add(newInstructor);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            PopulateAssignedSubjectData(_context, newInstructor);
            return Page();
        }
    }
}