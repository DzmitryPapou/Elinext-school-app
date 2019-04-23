using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using School.DAL.Entities;

namespace School.DAL.EF
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var teachers = new Teacher[]
            {
                new Teacher { FirstMidName = "Kim",     LastName = "Abercrombie"
                    },
                new Teacher { FirstMidName = "Fadi",    LastName = "Fakhouri"
                    },
                new Teacher { FirstMidName = "Roger",   LastName = "Harui" },
                new Teacher { FirstMidName = "Candace", LastName = "Kapoor" },
                new Teacher { FirstMidName = "Roger",   LastName = "Zheng" }
            };

            foreach (Teacher i in teachers)
            {
                context.Teachers.Add(i);
            }
            context.SaveChanges();

            var schoolclasses = new SchoolClass[]
            {
                new SchoolClass { Name = "1A",
                    TeacherID  = teachers.Single( i => i.LastName == "Abercrombie").ID },
                new SchoolClass { Name = "2Б",
                    TeacherID  = teachers.Single( i => i.LastName == "Fakhouri").ID },
                new SchoolClass { Name = "3В",
                    TeacherID  = teachers.Single( i => i.LastName == "Harui").ID },
                new SchoolClass { Name = "4Г",
                    TeacherID  = teachers.Single( i => i.LastName == "Kapoor").ID }
            };

            foreach (SchoolClass d in schoolclasses)
            {
                context.SchoolClasses.Add(d);
            }
            context.SaveChanges();

            var students = new Student[]
          {
                new Student { FirstMidName = "Carson",   LastName = "Alexander", Birthday = DateTime.Parse("2010-09-01"), Sex = "male",StudentsClassID = schoolclasses.Single(i => i.Name == "1A").SchoolClassID,
                    EnrollmentDate = DateTime.Parse("2010-09-01") },
                new Student { FirstMidName = "Meredith", LastName = "Alonso",Birthday = DateTime.Parse("2010-09-01"),Sex = "male",StudentsClassID = schoolclasses.Single(i => i.Name == "2Б").SchoolClassID,
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Arturo",   LastName = "Anand",Birthday = DateTime.Parse("2010-09-01"),Sex = "male",StudentsClassID = schoolclasses.Single(i => i.Name == "3В").SchoolClassID,
                    EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstMidName = "Gytis",    LastName = "Barzdukas",Birthday = DateTime.Parse("2010-09-01"),Sex = "male",StudentsClassID = schoolclasses.Single(i => i.Name == "4Г").SchoolClassID,
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Yan",      LastName = "Li",Birthday = DateTime.Parse("2010-09-01"),Sex = "male",StudentsClassID = schoolclasses.Single(i => i.Name == "4Г").SchoolClassID,
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Peggy",    LastName = "Justice",Birthday = DateTime.Parse("2010-09-01"),Sex = "male",StudentsClassID = schoolclasses.Single(i => i.Name == "1A").SchoolClassID,
                    EnrollmentDate = DateTime.Parse("2011-09-01") },
                new Student { FirstMidName = "Laura",    LastName = "Norman",Birthday = DateTime.Parse("2010-09-01"),Sex = "female",StudentsClassID = schoolclasses.Single(i => i.Name == "1A").SchoolClassID,
                    EnrollmentDate = DateTime.Parse("2013-09-01") },
                
          };

            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            var subjects = new Subject[]
            {
               new Subject {SubjectID = 1050, Title = "Chemistry",
                    SchoolClassID = schoolclasses.Single( s => s.Name == "1A").SchoolClassID
                },
                new Subject {SubjectID = 4022, Title = "Microeconomics",
                    SchoolClassID = schoolclasses.Single( s => s.Name == "1A").SchoolClassID
                },
                new Subject {SubjectID = 4041, Title = "Macroeconomics",
                    SchoolClassID = schoolclasses.Single( s => s.Name == "1A").SchoolClassID
                },
                new Subject {SubjectID = 1045, Title = "Calculus",
                    SchoolClassID = schoolclasses.Single( s => s.Name == "1A").SchoolClassID
                },
                new Subject {SubjectID = 3141, Title = "Trigonometry",
                    SchoolClassID = schoolclasses.Single( s => s.Name == "2Б").SchoolClassID
                },
                new Subject {SubjectID = 2021, Title = "Composition",
                    SchoolClassID = schoolclasses.Single( s => s.Name == "3В").SchoolClassID
                },
                new Subject {SubjectID = 2042, Title = "Literature",
                    SchoolClassID = schoolclasses.Single( s => s.Name == "4Г").SchoolClassID
                },
            };

            foreach (Subject c in subjects)
            {
                context.Subjects.Add(c);
            }
            context.SaveChanges();

            var positionAssignments = new PositionAssignment[]
            {
               new PositionAssignment {
                    TeacherID = teachers.Single( i => i.LastName == "Fakhouri").ID,
                    Position = "Director" },
                new PositionAssignment {
                    TeacherID = teachers.Single( i => i.LastName == "Harui").ID,
                    Position = "HeadTeacher" },
                new PositionAssignment {
                    TeacherID = teachers.Single( i => i.LastName == "Kapoor").ID,
                    Position = "Teacher" },
            };

            foreach (PositionAssignment o in positionAssignments)
            {
                context.PositionAssignments.Add(o);
            }
            context.SaveChanges();

            var subjectTeachers = new SubjectAssignment[]
            {
                new SubjectAssignment {
                    SubjectID = subjects.Single(c => c.Title == "Chemistry" ).SubjectID,
                    TeacherID = teachers.Single(i => i.LastName == "Kapoor").ID
                    },
                new SubjectAssignment {
                    SubjectID = subjects.Single(c => c.Title == "Chemistry" ).SubjectID,
                    TeacherID = teachers.Single(i => i.LastName == "Harui").ID
                    },
                new SubjectAssignment {
                    SubjectID = subjects.Single(c => c.Title == "Microeconomics" ).SubjectID,
                    TeacherID = teachers.Single(i => i.LastName == "Zheng").ID
                    },
                new SubjectAssignment {
                    SubjectID = subjects.Single(c => c.Title == "Macroeconomics" ).SubjectID,
                    TeacherID = teachers.Single(i => i.LastName == "Zheng").ID
                    },
                new SubjectAssignment {
                    SubjectID = subjects.Single(c => c.Title == "Calculus" ).SubjectID,
                    TeacherID = teachers.Single(i => i.LastName == "Fakhouri").ID
                    },
                new SubjectAssignment {
                    SubjectID = subjects.Single(c => c.Title == "Trigonometry" ).SubjectID,
                    TeacherID = teachers.Single(i => i.LastName == "Harui").ID
                    },
                new SubjectAssignment {
                    SubjectID = subjects.Single(c => c.Title == "Composition" ).SubjectID,
                    TeacherID = teachers.Single(i => i.LastName == "Abercrombie").ID
                    },
                new SubjectAssignment {
                    SubjectID = subjects.Single(c => c.Title == "Literature" ).SubjectID,
                    TeacherID = teachers.Single(i => i.LastName == "Abercrombie").ID
                    },
            };

            foreach (SubjectAssignment ci in subjectTeachers)
            {
                context.SubjectAssignments.Add(ci);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").ID,
                    SubjectID = subjects.Single(c => c.Title == "Chemistry" ).SubjectID,

                },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").ID,
                    SubjectID = subjects.Single(c => c.Title == "Microeconomics" ).SubjectID,

                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").ID,
                    SubjectID = subjects.Single(c => c.Title == "Macroeconomics" ).SubjectID,

                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").ID,
                    SubjectID = subjects.Single(c => c.Title == "Calculus" ).SubjectID,

                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").ID,
                    SubjectID = subjects.Single(c => c.Title == "Trigonometry" ).SubjectID,

                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alonso").ID,
                    SubjectID = subjects.Single(c => c.Title == "Composition" ).SubjectID,

                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").ID,
                    SubjectID = subjects.Single(c => c.Title == "Chemistry" ).SubjectID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").ID,
                    SubjectID = subjects.Single(c => c.Title == "Microeconomics").SubjectID,

                    },
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Barzdukas").ID,
                    SubjectID = subjects.Single(c => c.Title == "Chemistry").SubjectID,

                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Li").ID,
                    SubjectID = subjects.Single(c => c.Title == "Composition").SubjectID,

                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Justice").ID,
                    SubjectID = subjects.Single(c => c.Title == "Literature").SubjectID,

                    }
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.SingleOrDefault(s => s.Student.ID == e.StudentID &&
                                                                                    s.Subject.SubjectID == e.SubjectID);
                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }
           context.SaveChanges();
           
        }
    }
}
