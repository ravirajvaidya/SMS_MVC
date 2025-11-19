using Microsoft.EntityFrameworkCore;
using SMS_MVC.Models.Model_Tables;

namespace SMS_MVC.Models
{
    public class SMSDBContext : DbContext
    {
        public SMSDBContext(DbContextOptions<SMSDBContext> options) : base(options)
        {
        }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Subjects> Subjects { get; set; }

        public DbSet<Users> Users { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Students> Students { get; set; }

        public DbSet<Classes> Classes { get; set; }
        public DbSet<ClassAndSubject> ClassAndSubject { get; set; }
        public DbSet<TeacherAndClass> TeacherAndClass { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 👇 You add configurations here
            modelBuilder.Entity<ClassAndSubject>()
                .HasKey(cs => new { cs.ClassId, cs.SubjectId });

            modelBuilder.Entity<TeacherAndClass>()
                .HasKey(cs => new { cs.ClassId, cs.TeacherId });
        }
    }
}
