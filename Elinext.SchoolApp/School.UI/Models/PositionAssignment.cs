using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.UI.Models
{
    public class PositionAssignment
    {
        [Key]
        public int TeacherID { get; set; }
        [StringLength(50)]
        [Display(Name = "Position")]
        public string Position { get; set; }

        public Teacher Teacher { get; set; }
    }
}