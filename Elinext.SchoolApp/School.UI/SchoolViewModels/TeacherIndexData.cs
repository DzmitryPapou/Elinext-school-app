using System.Collections.Generic;
using School.DAL.Entities;


namespace School.UI.SchoolViewModels
{
    public class TeacherIndexData
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}