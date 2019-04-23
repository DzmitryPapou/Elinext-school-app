using System.Collections.Generic;


namespace School.DAL.Entities.SchoolViewModels
{
    public class TeacherIndexData
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}