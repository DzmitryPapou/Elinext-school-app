using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.UI.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int SubjectID { get; set; }
        public int StudentID { get; set; }

        public Subject Subject { get; set; }
        public Student Student { get; set; }
    }
}