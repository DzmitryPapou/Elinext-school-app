using School.UI.Filters;
using School.UI.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace School.UI.Pages.Students
{
    [HandleSqlException]
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public IndexModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        public PaginatedList<Student> Student { get; set; }

        public async Task OnGetAsync(string sortOrder,
    string currentFilter, string searchString, int? pageIndex)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSort"] = sortOrder == "Date";
            ViewData["SexSort"] = sortOrder == "Sex";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            IQueryable<Student> studentIQ = from s in _context.Students
                                            select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                studentIQ = studentIQ.Where(s => s.LastName.Contains(searchString));
                //|| s.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentIQ = studentIQ.OrderByDescending(s => s.Birthday);
                    break;
                case "Sex":
                    studentIQ = studentIQ.OrderBy(s => s.Sex);
                    break;
                default:
                    studentIQ = studentIQ.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 4;
            Student = await PaginatedList<Student>.CreateAsync(
                studentIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
