using System.Collections.Generic;

namespace School.UI.Models
{
    public class SchoolClass
    {
        public int SchoolClassID { get; set; }

        public string Name { get; set; }
        

        public int? TeacherID { get; set; }

        public Teacher ClassTeacher { get; set; }
        public ICollection<Subject> Subjects { get; set; }
    }
}