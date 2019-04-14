using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.UI.Models.SchoolViewModels
{
    public class TeacherIndexData
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}