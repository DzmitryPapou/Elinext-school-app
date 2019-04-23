using System;
using School.DAL.Entities;
using School.DAL.Interfaces;

namespace School.BLL
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<SchoolClass> SchoolClasses { get; }
        IRepository<Student> Students { get; }
        IRepository<Subject> Subjects { get; }
        IRepository<Teacher> Teachers { get; }
        void Save();
        void Dispose();
    }
}