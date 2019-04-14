using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using School.UI.Models;

namespace School.UI.Pages.SchoolClasses
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public IndexModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<SchoolClass> SchoolClass { get;set; }

        public async Task OnGetAsync()
        {
            SchoolClass = await _context.SchoolClasses
                .Include(d => d.ClassTeacher).ToListAsync();
        }
    }
}
