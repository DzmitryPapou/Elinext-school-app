using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.BLL;
using School.DAL.Entities;


namespace School.UI.Controllers
{
    public class SchoolClassesController : Controller
    {
        private readonly IUnitOfWork _context;

        public SchoolClassesController(IUnitOfWork context)
        {
            _context = context;
        }

        // GET: SchoolClasses
        public  IActionResult Index()
        {
            var schoolContext = _context.SchoolClasses.GetAll();
            return View(schoolContext);
        }

        // GET: SchoolClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string query = "SELECT * FROM SchoolClass WHERE SchoolClassID = {0}";
            var schoolClass = await _context.SchoolClasses.GetAll()
                .FromSql(query, id)
                .AsNoTracking()
                .FirstOrDefaultAsync();



            if (schoolClass == null)
            {
                return NotFound();
            }

            return View(schoolClass);
        }

        // GET: SchoolClasses/Create
        public IActionResult Create()
        {
            ViewData["TeacherID"] = new SelectList(_context.Teachers.GetAll(), "ID", "FullName");
            return View();
        }

        // POST: SchoolClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SchoolClassID,Name,TeacherID")] SchoolClass schoolClass)
        {
            if (ModelState.IsValid)
            {
                _context.SchoolClasses.Add(schoolClass);
                _context.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers.GetAll(), "ID", "FullName", schoolClass.TeacherID);
            return View(schoolClass);
        }

        // GET: SchoolClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolClass = await _context.SchoolClasses.GetAll()
                .Include(i => i.ClassTeacher)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SchoolClassID == id);

            if (schoolClass == null)
            {
                return NotFound();
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers.GetAll(), "ID", "FullName", schoolClass.TeacherID);
            return View(schoolClass);
        }

        // POST: SchoolClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolClassToUpdate = _context.SchoolClasses.GetAll().Include(i => i.ClassTeacher)
                .FirstOrDefaultAsync(m => m.SchoolClassID == id);

            if (schoolClassToUpdate == null)
            {
                var deletedSchoolClass = new SchoolClass();
                await TryUpdateModelAsync(deletedSchoolClass);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                ViewData["TeacherID"] = new SelectList(_context.Teachers.GetAll(), "ID", "FullName",
                    deletedSchoolClass.TeacherID);
                return View(deletedSchoolClass);
            }

            if (await TryUpdateModelAsync<SchoolClass>(
                await schoolClassToUpdate,
                "",
                s => s.Name, s => s.TeacherID))
            {
                try
                {
                    _context.Save();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = exceptionEntry.Entity as SchoolClass;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (SchoolClass)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                        }
                        if (databaseValues.TeacherID != clientValues.TeacherID)
                        {
                            var databaseTeacher = await _context.Teachers.GetAll().FirstOrDefaultAsync(i => i.ID == databaseValues.TeacherID);
                            ModelState.AddModelError("TeacherID", $"Current value: {databaseTeacher?.FullName}");
                        }

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you got the original value. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to edit this record, click "
                                + "the Save button again. Otherwise click the Back to List hyperlink.");
                    }
                }
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers.GetAll(), "ID", "FullName", schoolClassToUpdate.Result.TeacherID);
            return View(schoolClassToUpdate);
        }

        // GET: SchoolClasses/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolClass = await _context.SchoolClasses.GetAll()
                .Include(d => d.ClassTeacher)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SchoolClassID == id);
            if (schoolClass == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(schoolClass);
        }
        // POST: SchoolClasses/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(SchoolClass schoolClass)
        {
            try
            {
                if (await _context.SchoolClasses.GetAll().AnyAsync(m => m.SchoolClassID == schoolClass.SchoolClassID))
                {
                    _context.SchoolClasses.Delete(schoolClass.SchoolClassID);
                    _context.Save();
                }
               
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException /* ex*/ )
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = schoolClass.SchoolClassID });
            }
        }

        private bool SchoolClassExists(int id)
        {
            return _context.SchoolClasses.GetAll().Any(e => e.SchoolClassID == id);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
