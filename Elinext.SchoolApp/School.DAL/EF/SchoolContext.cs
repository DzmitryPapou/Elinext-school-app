using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using School.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace School.DAL.EF
{
    public class SchoolContext : IdentityDbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<PositionAssignment> PositionAssignments { get; set; }
        public DbSet<SubjectAssignment> SubjectAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().ToTable("Subject");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<SchoolClass>().ToTable("SchoolClass");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<PositionAssignment>().ToTable("PositionAssignment");
            modelBuilder.Entity<SubjectAssignment>().ToTable("SubjectAssignment");

            modelBuilder.Entity<SubjectAssignment>()
                .HasKey(c => new { c.SubjectID, c.TeacherID });
            base.OnModelCreating(modelBuilder);
        }
    }

    public class SchoolContextContextFactory : IDesignTimeDbContextFactory<SchoolContext>
    {
        public SchoolContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolContext>();
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=CUtest6;Trusted_Connection=True;", b => b.MigrationsAssembly("School.DAL"));

            return new SchoolContext(optionsBuilder.Options);
        }
    }
}