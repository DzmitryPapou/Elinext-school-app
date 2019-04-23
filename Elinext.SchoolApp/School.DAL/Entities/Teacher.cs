using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.DAL.Entities
{
    public class Teacher 
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstMidName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => LastName + ", " + FirstMidName;

        public ICollection<SubjectAssignment> SubjectAssignments { get; set; }
        public PositionAssignment PositionAssignments { get; set; }
    }
}