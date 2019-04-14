using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.UI.Models;

namespace School.UI.Pages.Subjects
{
    public class CreateModel : SchoolClassNamePageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public CreateModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateSubjectsDropDownList(_context);
            Subject = new Subject
            {
                SubjectID = 1,
                Title = "Algebra 3"
            };
            return Page();
        }

        [BindProperty]
        public Subject Subject { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyCourse = new Subject();

            if (await TryUpdateModelAsync<Subject>(
                 emptyCourse,
                 "subject",   // Prefix for form value.
                 s => s.SubjectID, s => s.SchoolClassID, s => s.Title))
            {
                _context.Subjects.Add(emptyCourse);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Select SchoolClassID if TryUpdateModelAsync fails.
            PopulateSubjectsDropDownList(_context, emptyCourse.SchoolClassID);
            return Page();
        }
    }
}