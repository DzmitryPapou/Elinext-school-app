using School.UI.Data;
using School.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace School.UI.Pages.SchoolClasses
{
    public class EditModel : PageModel
    {
        private readonly School.UI.Data.SchoolContext _context;

        public EditModel(School.UI.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SchoolClass SchoolClass { get; set; }
      
        public SelectList TeacherNameSL { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            SchoolClass = await _context.SchoolClasses
                .Include(d => d.ClassTeacher)  // eager loading
                .AsNoTracking()                 // tracking not required
                .FirstOrDefaultAsync(m => m.SchoolClassID == id);

            if (SchoolClass == null)
            {
                return NotFound();
            }

            // Use strongly typed data rather than ViewData.
            TeacherNameSL = new SelectList(_context.Teachers,
                "ID", "FirstMidName");

            return Page();
        }



        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var departmentToUpdate = await _context.SchoolClasses
                .Include(i => i.ClassTeacher)
                .FirstOrDefaultAsync(m => m.SchoolClassID == id);

            // null means SchoolClass was deleted by another user.
            if (departmentToUpdate == null)
            {
                return await HandleDeletedDepartment();
            }

         

            if (await TryUpdateModelAsync<SchoolClass>(
                departmentToUpdate,
                "SchoolClass",
                s => s.Name, s => s.TeacherID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (SchoolClass)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save. " +
                            "The schoolclass was deleted by another user.");
                        return Page();
                    }

                    var dbValues = (SchoolClass)databaseEntry.ToObject();
                    await setDbErrorMessage(dbValues, clientValues, _context);

                    
                }
            }

            TeacherNameSL = new SelectList(_context.Teachers,
                "ID", "FullName", departmentToUpdate.TeacherID);

            return Page();
        }

        private async Task<IActionResult> HandleDeletedDepartment()
        {
            SchoolClass deletedDepartment = new SchoolClass();
            // ModelState contains the posted data because of the deletion error and will overide the SchoolClass instance values when displaying Page().
            ModelState.AddModelError(string.Empty,
                "Unable to save. The schoolclass was deleted by another user.");
            TeacherNameSL = new SelectList(_context.Teachers, "ID", "FullName", SchoolClass.TeacherID); 
            return Page();
        }

         private async Task setDbErrorMessage(SchoolClass dbValues,
                SchoolClass clientValues, SchoolContext context)
        {

            if (dbValues.Name != clientValues.Name)
            {
                ModelState.AddModelError("SchoolClass.Name",
                    $"Current value: {dbValues.Name}");
            }
            if (dbValues.TeacherID != clientValues.TeacherID)
            {
                Teacher dbInstructor = await _context.Teachers
                   .FirstOrDefaultAsync(i => i.ID == dbValues.TeacherID);
                ModelState.AddModelError("SchoolClass.TeacherID",
                    $"Current value: {dbInstructor?.FullName}");
            }

            ModelState.AddModelError(string.Empty,
                "The record you attempted to edit "
              + "was modified by another user after you. The "
              + "edit operation was canceled and the current values in the database "
              + "have been displayed. If you still want to edit this record, click "
              + "the Save button again.");
        }
    }
}