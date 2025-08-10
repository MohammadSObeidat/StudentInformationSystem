using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SIS.Data
{
    public class SisContext : IdentityDbContext<ApplicationUser> // DbContext
    {
        public DbSet<Course> Course { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentSection> StudentSection { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=SIS; Integrated Security=True; TrustServerCertificate=True");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // المفتاح المركب
            modelBuilder.Entity<StudentSection>()
                .HasKey(ss => new { ss.StudentId, ss.SectionId });

            modelBuilder.Entity<StudentSection>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSection)
                .HasForeignKey(ss => ss.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentSection>()
                .HasOne(ss => ss.Section)
                .WithMany(s => s.StudentSection)
                .HasForeignKey(ss => ss.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Grade -> Student (No Cascade)
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Grade -> Section (Cascade مسموح بها هنا)
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Section)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique index for StudentNumber
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.StudentNumber)
                .IsUnique();

            // Unique index for SectionNumber
            modelBuilder.Entity<Section>()
                .HasIndex(e => e.SectionNumber)
                .IsUnique();

            // Unique index for InstructorNumber
            modelBuilder.Entity<Instructor>()
                .HasIndex(e => e.InstructorNumber)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
