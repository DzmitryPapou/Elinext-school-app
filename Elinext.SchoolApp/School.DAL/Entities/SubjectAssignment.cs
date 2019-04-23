namespace School.DAL.Entities
{
    public class SubjectAssignment
    {
        public int TeacherID { get; set; }
        public int SubjectID { get; set; }
        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
    }
}