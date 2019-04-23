using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.BLL;
using School.DAL.Entities;

namespace School.UI.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly IUnitOfWork _context;

        public SubjectsController(IUnitOfWork context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            var subjects = _context.Subjects.GetAll()
                .Include(c => c.SchoolClass)
                .AsNoTracking();
            return View(await subjects.ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.GetAll()
                .Include(c => c.SchoolClass)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SubjectID == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            PopulateSchoolClassesDropDownList();
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("SubjectID,SchoolClassID,Title")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Subjects.Add(subject);
                _context.Save();
                return RedirectToAction(nameof(Index));
            }
            PopulateSchoolClassesDropDownList(subject.SchoolClassID);
            return View(subject);
        }

        // GET: Subjects/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SubjectID == id);
            if (subject == null)
            {
                return NotFound();
            }
            PopulateSchoolClassesDropDownList(subject.SchoolClassID);
            return View(subject);
        }

        // POST: Subjects/Edit/5
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

            var subjectToUpdate = await _context.Subjects.GetAll()
                .FirstOrDefaultAsync(c => c.SubjectID == id);

            if (await TryUpdateModelAsync<Subject>(subjectToUpdate,
                "",
                 c => c.SchoolClassID, c => c.Title))
            {
                try
                {
                     _context.Save();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateSchoolClassesDropDownList(subjectToUpdate.SchoolClassID);
            return View(subjectToUpdate);
        }

        private void PopulateSchoolClassesDropDownList(object selectedSchoolClass = null)
        {
            var schoolClassesQuery = from d in _context.SchoolClasses.GetAll()
                                   orderby d.Name
                                   select d;
            ViewBag.SchoolClassID = new SelectList(schoolClassesQuery.AsNoTracking(), "SchoolClassID", "Name", selectedSchoolClass);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.GetAll()
                .Include(c => c.SchoolClass)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SubjectID == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var subject =  _context.Subjects.GetById(id);
            _context.Subjects.Delete(subject.SubjectID);
            _context.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.GetAll().Any(e => e.SubjectID == id);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
