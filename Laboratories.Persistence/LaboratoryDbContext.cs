using Laboratories.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Persistence
{
     public class LaboratoryDbContext : DbContext
    {
        public LaboratoryDbContext(): base("LaboratoryConnection")
        {
         
 
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Complex> Complices { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AppSeting> AppSetings { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ScheduleHed> ScheduleHed { get; set; }
        public DbSet<ScheduleDet> ScheduleDet { get; set; }
        public DbSet<TeacherMovment> TeacherMovments { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Experiments> Experiments { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentGradesDet> StudentGradesDet { get; set; }
        public DbSet<StudentGradesHed> StudentGradesHed { get; set; }

        public DbSet<StudyCourse> StudyCourses { get; set; }

        public DbSet<Screen> Screens { get; set; }
        public DbSet<ScreenRole> ScreenRoles { get; set; }

        public DbSet<UserRole> userRoles { get; set; }

        public DbSet<UserSchools> userSchools { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Screen>()
                      .Property(e => e.screen_id)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
