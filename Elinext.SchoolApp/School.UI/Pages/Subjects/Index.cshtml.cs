using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using School.UI.Data;
using School.UI.Models;

namespace School.UI.Pages.Subjects
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public IndexModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<Subject> Subject { get;set; }

        public async Task OnGetAsync()
        {
            Subject = await _context.Subjects
                .Include(c => c.SchoolClass)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
