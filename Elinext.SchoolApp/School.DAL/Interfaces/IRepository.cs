using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School.DAL.Entities;

namespace School.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        void Add(T item);
        void Delete(int? id);
        T GetById(int? id);
        void Update(T item);

    }
}
