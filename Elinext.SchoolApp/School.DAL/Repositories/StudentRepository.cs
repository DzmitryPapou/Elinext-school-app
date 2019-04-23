using System.Linq;
using Microsoft.EntityFrameworkCore;
using School.DAL.EF;
using School.DAL.Entities;
using School.DAL.Interfaces;

namespace School.DAL.Repositories
{
    public class StudentRepository : IRepository<Student>
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public IQueryable<Student> GetAll() => _context.Students;

        public void Add(Student item) => _context.Students.Add(item);

        public Student GetById(int? id) => _context.Students.Find(id);

        public void Update(Student item)
        {
            _context.Students.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int? id)
        {
            var students = _context.Students.Find(id);
            if (students != null)
                _context.Students.Remove(students);
        }
    }
}
