using System;
using School.DAL.EF;
using School.DAL.Entities;
using School.DAL.Interfaces;
using School.DAL.Repositories;

namespace School.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolContext context;
        private SchoolClassRepository schoolClassRepository;
        private StudentRepository studentRepository;
        private SubjectRepository subjectRepository;
        private TeacherRepository teacherRepository;

        public UnitOfWork(SchoolContext context)
        {
            this.context = context;
        }


        public IRepository<SchoolClass> SchoolClasses
        {
            get
            {
                if (schoolClassRepository == null)
                    schoolClassRepository = new SchoolClassRepository(context);
                return schoolClassRepository;
            }
        }
        public IRepository<Student> Students
        {
            get
            {
                if (studentRepository == null)
                    studentRepository = new StudentRepository(context);
                return studentRepository;
            }
        }



        public IRepository<Subject> Subjects
        {
            get
            {
                if (subjectRepository == null)
                    subjectRepository = new SubjectRepository(context);
                return subjectRepository;
            }
        }

        public IRepository<Teacher> Teachers
        {
            get
            {
                if (teacherRepository == null)
                    teacherRepository = new TeacherRepository(context);
                return teacherRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

