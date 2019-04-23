using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.BLL;
using School.DAL.Entities;
using School.UI.SchoolViewModels;


namespace School.UI.Controllers
{
    public class TeachersController : Controller
    {
        private readonly IUnitOfWork _context;

        public TeachersController(IUnitOfWork context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(int? id, int? subjectID)
        {
            var viewModel = new TeacherIndexData
            {
                Teachers = await _context.Teachers.GetAll()
                    .Include(i => i.PositionAssignments)
                    .Include(i => i.SubjectAssignments)
                    .ThenInclude(i => i.Subject)
                    .ThenInclude(i => i.SchoolClass)
                    .OrderBy(i => i.LastName)
                    .ToListAsync()
            };

            if (id != null)
            {
                ViewData["TeacherID"] = id.Value;
                var teacher = viewModel.Teachers.Single(i => i.ID == id.Value);
                viewModel.Subjects = teacher.SubjectAssignments.Select(s => s.Subject);
            }

            if (subjectID != null)
            {
                ViewData["SubjectID"] = subjectID.Value;
                var selectedSubject = viewModel.Subjects.Single(x => x.SubjectID == subjectID);
                _context.Subjects.Update(selectedSubject);
                viewModel.Enrollments = selectedSubject.Enrollments;

            }

            return View(viewModel);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.GetAll()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            var teacher = new Teacher {SubjectAssignments = new List<SubjectAssignment>()};
            PopulateAssignedSubjectData(teacher);
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,PositionAssignments")] Teacher teacher, string[] selectedSubjects)
        {
            if (selectedSubjects != null)
            {
                teacher.SubjectAssignments = new List<SubjectAssignment>();
                foreach (var subject in selectedSubjects)
                {
                    var subjectToAdd = new SubjectAssignment { TeacherID = teacher.ID, SubjectID = int.Parse(subject) };
                    teacher.SubjectAssignments.Add(subjectToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Teachers.Add(teacher);
                 _context.Save();
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedSubjectData(teacher);
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.GetAll()
                .Include(i => i.PositionAssignments)
                .Include(i => i.SubjectAssignments).ThenInclude(i => i.Subject)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }
            PopulateAssignedSubjectData(teacher);
            return View(teacher);
        }

        private void PopulateAssignedSubjectData(Teacher teacher)
        {
            var allSubjects = _context.Subjects.GetAll();
            var teacherSubjects = new HashSet<int>(teacher.SubjectAssignments.Select(c => c.SubjectID));
            var viewModel = allSubjects.Select(subject => new AssignedSubjectData {SubjectID = subject.SubjectID, Title = subject.Title, Assigned = teacherSubjects.Contains(subject.SubjectID)}).ToList();
            ViewData["Subjects"] = viewModel;
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedSubjects)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherToUpdate = await _context.Teachers.GetAll()
                .Include(i => i.PositionAssignments)
                .Include(i => i.SubjectAssignments)
                    .ThenInclude(i => i.Subject)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Teacher>(
                teacherToUpdate,
                "",
                i => i.FirstMidName, i => i.LastName,  i => i.PositionAssignments))
            {
                if (String.IsNullOrWhiteSpace(teacherToUpdate.PositionAssignments?.Position))
                {
                    teacherToUpdate.PositionAssignments = null;
                }
                UpdateTeacherSubjects(selectedSubjects, teacherToUpdate);
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
            UpdateTeacherSubjects(selectedSubjects, teacherToUpdate);
            PopulateAssignedSubjectData(teacherToUpdate);
            return View(teacherToUpdate);
        }
        
        private void UpdateTeacherSubjects(string[] selectedSubjects, Teacher teacherToUpdate)
        {
            if (selectedSubjects == null)
            {
                teacherToUpdate.SubjectAssignments = new List<SubjectAssignment>();
                return;
            }

            var selectedSubjectsHS = new HashSet<string>(selectedSubjects);
            var teacherSubjects = new HashSet<int>
                (teacherToUpdate.SubjectAssignments.Select(c => c.Subject.SubjectID));
            foreach (var subject in _context.Subjects.GetAll())
            {
                if (selectedSubjectsHS.Contains(subject.SubjectID.ToString()))
                {
                    if (!teacherSubjects.Contains(subject.SubjectID))
                    {
                        teacherToUpdate.SubjectAssignments.Add(new SubjectAssignment { TeacherID = teacherToUpdate.ID, SubjectID = subject.SubjectID });
                    }
                }
                else
                {

                    if (teacherSubjects.Contains(subject.SubjectID))
                    {
                        SubjectAssignment subjectToRemove = teacherToUpdate.SubjectAssignments.FirstOrDefault(i => i.SubjectID == subject.SubjectID);
                        _context.Subjects.Delete(subjectToRemove.SubjectID);
                    }
                }
            }
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.GetAll()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.GetAll()
                .Include(i => i.SubjectAssignments)
                .SingleAsync(i => i.ID == id);

            var schoolClasses = await _context.SchoolClasses.GetAll()
                .Where(d => d.TeacherID == id)
                .ToListAsync();
            schoolClasses.ForEach(d => d.TeacherID = null);

            _context.Teachers.Delete(teacher.ID);
             _context.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SchoolList()
        {
            var schoolList = await _context.Students.GetAll()
                .Include(d => d.StudentsClass).ToListAsync();
            return View(schoolList);
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.GetAll().Any(e => e.ID == id);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
