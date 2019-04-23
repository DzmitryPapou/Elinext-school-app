using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.DAL.Entities
{
    public class Student
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Age")]
        public int Age
        {
            get
            {
                var now = DateTime.Today;
                var age = now.Year - Birthday.Year;
                if (Birthday > now.AddYears(-age)) age--;
                return age;
            }
        }
        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => LastName + ", " + FirstMidName;

        public int? StudentsClassID { get; set; }

        public SchoolClass StudentsClass { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}