using System.Linq;
using Microsoft.EntityFrameworkCore;
using School.DAL.EF;
using School.DAL.Entities;
using School.DAL.Interfaces;

namespace School.DAL.Repositories
{
    public class TeacherRepository : IRepository<Teacher>
    {
        private readonly SchoolContext _context;
        
        public TeacherRepository(SchoolContext context)
        {
            _context = context;
          
        }

        public void Add(Teacher item) => _context.Teachers.AddAsync(item);

        public Teacher GetById(int? id)
        {
            var teacher = _context.Teachers.Find(id);
            _context.Entry(teacher).Reference(x => x.PositionAssignments).Load();
            return _context.Teachers.Find(id);
        }

        public IQueryable<Teacher> GetAll()
        {
           
            foreach (var i in _context.Teachers)
            {
                _context.Entry(i).Reference(x => x.PositionAssignments).Load();
            }
            return _context.Teachers;
        }

        public void Delete(int? id)
        {
            var teachers = _context.Teachers.Find(id);
            if (teachers != null)
                _context.Teachers.Remove(teachers);
        }

        public void Update(Teacher teacher)
        {
            _context.Teachers.Attach(teacher);
            _context.Entry(teacher).State = EntityState.Modified;
        }

    }
}
