using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School.UI.Models;

namespace School.UI.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public CreateModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        
        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyStudent = new Student();

            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",   // Prefix for form value.
                s => s.FirstMidName, s => s.LastName, s=> s.Birthday, s => s.EnrollmentDate, s => s.Birthday, s => s.Age, s => s.Sex))
            {
                _context.Students.Add(emptyStudent);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return null;
        }
    }
}