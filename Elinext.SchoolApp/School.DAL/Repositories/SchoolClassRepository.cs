using System.Linq;
using Microsoft.EntityFrameworkCore;
using School.DAL.EF;
using School.DAL.Entities;
using School.DAL.Interfaces;

namespace School.DAL.Repositories
{
    public class SchoolClassRepository : IRepository<SchoolClass>
    { 
        private readonly SchoolContext _context;

        public SchoolClassRepository(SchoolContext context)
        {
            _context = context;
        }
       

        public IQueryable<SchoolClass> GetAll() => _context.SchoolClasses;

        public void Add(SchoolClass item) => _context.SchoolClasses.Add(item);

        public SchoolClass GetById(int? id) => _context.SchoolClasses.Find(id);

        public void Update(SchoolClass item)
        {
            _context.SchoolClasses.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int? id)
        {
            var schoolClasses = _context.SchoolClasses.Find(id);
            if (schoolClasses != null)
                _context.SchoolClasses.Remove(schoolClasses);
        }

    }
}
