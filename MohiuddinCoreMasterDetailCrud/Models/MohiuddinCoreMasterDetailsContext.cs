using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MohiuddinCoreMasterDetailCrud.Models
{
    public partial class MohiuddinCoreMasterDetailsContext : DbContext
    {
        public MohiuddinCoreMasterDetailsContext() { }

        public MohiuddinCoreMasterDetailsContext(DbContextOptions<MohiuddinCoreMasterDetailsContext> options)
            : base(options) { }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentDetails> StudentDetails { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<CourseInstructor> CourseInstructor { get; set; }
        public virtual DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public virtual DbSet<InstructorDetails> InstructorDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A7F23CBDCC");
                entity.Property(e => e.CourseName).HasMaxLength(50).IsUnicode(false);

                entity.HasMany(c => c.Modules)
                    .WithOne(m => m.Course)
                    .HasForeignKey(m => m.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasKey(e => e.ModuleId).HasName("PK__Modules__2B7477A7403D382B");
                entity.Property(e => e.ModuleName).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Duration).IsRequired();
                entity.Property(e => e.CourseId).IsRequired();
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B993A2ECB17");
                entity.Property(e => e.Dob).HasColumnType("datetime");
                entity.Property(e => e.ImageUrl).IsUnicode(false);
                entity.Property(e => e.Mobile).HasMaxLength(14).IsUnicode(false);
                entity.Property(e => e.StudentName).HasMaxLength(50).IsUnicode(false);

                
            });

            modelBuilder.Entity<StudentDetails>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Student)
                    .WithOne(p => p.StudentDetails)
                    .HasForeignKey<StudentDetails>(d => d.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Administrator)
                .WithMany(i => i.Departments)
                .HasForeignKey(d => d.InstructorID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Instructor>()
                .HasMany(i => i.CourseInstructor)
                .WithOne(ci => ci.Instructor)
                .HasForeignKey(ci => ci.InstructorID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.CourseInstructor)
                .WithOne(ci => ci.Course)
                .HasForeignKey(ci => ci.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.OfficeAssignment)
                .WithOne(o => o.Instructor)
                .HasForeignKey<OfficeAssignment>(o => o.InstructorID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseInstructor>()
                .HasKey(ci => new { ci.CourseId, ci.InstructorID });

            modelBuilder.Entity<CourseInstructor>()
                .HasOne(ci => ci.Course)
                .WithMany(c => c.CourseInstructor)
                .HasForeignKey(ci => ci.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseInstructor>()
                .HasOne(ci => ci.Instructor)
                .WithMany(i => i.CourseInstructor)
                .HasForeignKey(ci => ci.InstructorID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.InstructorDetails)
                .WithOne(d => d.Instructor)
                .HasForeignKey<InstructorDetails>(d => d.InstructorID)
                .OnDelete(DeleteBehavior.Cascade);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
