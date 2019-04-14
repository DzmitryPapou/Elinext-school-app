using School.UI.Models;
using School.UI.Models.SchoolViewModels; 
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace School.UI.Pages.Teachers
{
    [Authorize(Roles = "Administrator,Teacher")]
    public class IndexModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public IndexModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        public TeacherIndexData Teacher { get; set; }


        public async Task OnGetAsync(int? id, int? subjectID)
        {
            Teacher = new TeacherIndexData
            {
                Teachers = await _context.Teachers
                    .Include(i => i.PositionAssignments)
                    .Include(i => i.SubjectAssignments)
                    .ThenInclude(i => i.Subject)
                    .ThenInclude(i => i.SchoolClass)
                    .Include(i => i.SubjectAssignments)
                    .ThenInclude(i => i.Subject)
                    .ThenInclude(i => i.Enrollments)
                    .ThenInclude(i => i.Student)
                    .AsNoTracking()
                    .OrderBy(i => i.LastName)
                    .ToListAsync()
            };

            if (id != null)
            {
                ViewData["TeacherID"] = id.Value;
                var teacher = Teacher.Teachers.Single(i => i.ID == id.Value);
                Teacher.Subjects = teacher.SubjectAssignments.Select(s => s.Subject);
            }

            if (subjectID != null)
            {
                ViewData["SubjectID"] = subjectID.Value;
                Teacher.Enrollments = Teacher.Subjects.Single(x => x.SubjectID == subjectID).Enrollments;
            }
        }
    }
}
