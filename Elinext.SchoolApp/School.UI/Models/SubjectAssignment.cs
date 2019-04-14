using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.UI.Models
{
    public class SubjectAssignment
    {
        public int TeacherID { get; set; }
        public int SubjectID { get; set; }
        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
    }
}