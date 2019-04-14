using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using School.UI.Models;

namespace School.UI.Pages.SchoolClasses
{

    public class CreateModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public CreateModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

       
        [BindProperty]
        public SchoolClass SchoolClass { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SchoolClasses.Add(SchoolClass);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}