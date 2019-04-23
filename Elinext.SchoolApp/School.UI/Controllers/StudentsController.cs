using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.BLL;
using School.DAL.Entities;

namespace School.UI.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _context;

        public StudentsController(IUnitOfWork context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageIndex)
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

            var students = from s in _context.Students.GetAll()
                           select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.Birthday);
                    break;
                case "Sex":
                    students = students.OrderByDescending(s => s.Sex);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 4;
            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageIndex ?? 1, pageSize));
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.GetAll()
                 .Include(s => s.Enrollments)
                     .ThenInclude(e => e.Subject)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            [Bind("EnrollmentDate,FirstMidName,LastName,Birthday,Sex")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Students.Add(student);
                    _context.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student =  _context.Students.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
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
            var studentToUpdate = await _context.Students.GetAll().FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "",
                s => s.FirstMidName, s => s.LastName, s => s.Birthday, s => s.EnrollmentDate, s => s.Age, s => s.Sex))
            {
                try
                {
                    _context.Save();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student =  _context.Students.GetById(id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Students.Delete(student.ID);
                _context.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool StudentExists(int id)
        {
            return _context.Students.GetAll().Any(e => e.ID == id);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
