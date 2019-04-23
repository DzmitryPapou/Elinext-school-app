using System.Linq;
using Microsoft.EntityFrameworkCore;
using School.DAL.EF;
using School.DAL.Entities;
using School.DAL.Interfaces;

namespace School.DAL.Repositories
{
    public class SubjectRepository : IRepository<Subject>
    {
        private readonly SchoolContext _context;

        public SubjectRepository(SchoolContext context)
        {
            _context = context;
        }



        public void Add(Subject item) => _context.Subjects.AddAsync(item);

        public Subject GetById(int? id) => _context.Subjects.Find(id);

        public IQueryable<Subject> GetAll() => _context.Subjects;

        public void Update(Subject subject)
        {
            _context.Entry(subject).Collection(x => x.Enrollments).Load();
            foreach (var enrollment in subject.Enrollments)
            {
                 _context.Entry(enrollment).Reference(x => x.Student).Load();
            }
            _context.Subjects.Attach(subject);
            _context.Entry(subject).State = EntityState.Modified;
        }

        public void Delete(int? id)
        {
            var subjects = _context.Subjects.Find(id);
            if (subjects != null)
                _context.Subjects.Remove(subjects);
        }

      
    }
}
