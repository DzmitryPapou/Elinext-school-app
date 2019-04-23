using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.DAL.Entities
{
    public class Subject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int SubjectID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

       public int SchoolClassID { get; set; }

        public SchoolClass SchoolClass { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<SubjectAssignment> SubjectAssignments { get; set; }
    }
}