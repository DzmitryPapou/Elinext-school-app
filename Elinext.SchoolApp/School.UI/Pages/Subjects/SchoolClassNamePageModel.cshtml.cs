using School.UI.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace School.UI.Pages.Subjects
{
    public class SchoolClassNamePageModel : PageModel
    {
        public SelectList SubjectNameSL { get; set; }

        public void PopulateSubjectsDropDownList(SchoolContext _context,
            object selectedSubject = null)
        {
            var subjectsQuery = from d in _context.SchoolClasses
                                   orderby d.Name // Sort by name.
                                   select d;

            SubjectNameSL = new SelectList(subjectsQuery.AsNoTracking(),
                        "SchoolClassID", "Name", selectedSubject);
        }
    }
}