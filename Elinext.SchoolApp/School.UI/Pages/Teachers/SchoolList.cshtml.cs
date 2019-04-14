using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School.UI.Data;
using School.UI.Models;

namespace School.UI.Pages
{
    public class SchoolList : PageModel
    {
        private readonly SchoolContext _context;

        public SchoolList(SchoolContext context)
        {
            _context = context;
        }

       

        public IList<Student> Students { get; set; }
       

        public async Task OnGetAsync()
        {
           Students = await _context.Students
                .Include(d => d.StudentsClass).ToListAsync();
         
        }
    }
}