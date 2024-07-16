﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MohiuddinCoreMasterDetailCrud.Models;

#nullable disable

namespace MohiuddinCoreMasterDetailCrud.Migrations
{
    [DbContext(typeof(MohiuddinCoreMasterDetailsContext))]
    [Migration("20240716080741_update-student")]
    partial class updatestudent
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.HasKey("CourseId")
                        .HasName("PK__Courses__C92D71A7F23CBDCC");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.CourseInstructor", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("InstructorID")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "InstructorID");

                    b.HasIndex("InstructorID");

                    b.ToTable("CourseInstructor");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"));

                    b.Property<decimal>("Budget")
                        .HasColumnType("money");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("InstructorID")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("DepartmentID");

                    b.HasIndex("InstructorID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentID"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("EnrollmentID");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Instructor", b =>
                {
                    b.Property<int>("InstructorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InstructorID"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("InstructorID");

                    b.ToTable("Instructors");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.InstructorDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<int>("InstructorID")
                        .HasColumnType("int");

                    b.Property<string>("InstructorPicture")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PermanentAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PresentAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("InstructorID")
                        .IsUnique();

                    b.ToTable("InstructorDetails");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Module", b =>
                {
                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ModuleId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ModuleId")
                        .HasName("PK__Modules__2B7477A7403D382B");

                    b.HasIndex("CourseId");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.OfficeAssignment", b =>
                {
                    b.Property<int>("InstructorID")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("InstructorID");

                    b.ToTable("OfficeAssignments");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(14)
                        .IsUnicode(false)
                        .HasColumnType("varchar(14)");

                    b.Property<int?>("StudentDetailsId")
                        .HasColumnType("int");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("StudentId")
                        .HasName("PK__Students__32C52B993A2ECB17");

                    b.HasIndex("CourseId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.StudentDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GuardianMobile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuardianName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PermanentAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PresentAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelationWithGuardian")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.ToTable("StudentDetails");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Course", b =>
                {
                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Department", "Department")
                        .WithMany("Courses")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.CourseInstructor", b =>
                {
                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Course", "Course")
                        .WithMany("CourseInstructor")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Instructor", "Instructor")
                        .WithMany("CourseInstructor")
                        .HasForeignKey("InstructorID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Department", b =>
                {
                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Instructor", "Administrator")
                        .WithMany("Departments")
                        .HasForeignKey("InstructorID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Administrator");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Enrollment", b =>
                {
                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.InstructorDetails", b =>
                {
                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Instructor", "Instructor")
                        .WithOne("InstructorDetails")
                        .HasForeignKey("MohiuddinCoreMasterDetailCrud.Models.InstructorDetails", "InstructorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Module", b =>
                {
                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Course", "Course")
                        .WithMany("Modules")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.OfficeAssignment", b =>
                {
                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Instructor", "Instructor")
                        .WithOne("OfficeAssignment")
                        .HasForeignKey("MohiuddinCoreMasterDetailCrud.Models.OfficeAssignment", "InstructorID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Student", b =>
                {
                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Course", "Course")
                        .WithMany("Students")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK__Students__Course__267ABA7A");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.StudentDetails", b =>
                {
                    b.HasOne("MohiuddinCoreMasterDetailCrud.Models.Student", "Student")
                        .WithOne("StudentDetails")
                        .HasForeignKey("MohiuddinCoreMasterDetailCrud.Models.StudentDetails", "StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Course", b =>
                {
                    b.Navigation("CourseInstructor");

                    b.Navigation("Enrollments");

                    b.Navigation("Modules");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Department", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Instructor", b =>
                {
                    b.Navigation("CourseInstructor");

                    b.Navigation("Departments");

                    b.Navigation("InstructorDetails")
                        .IsRequired();

                    b.Navigation("OfficeAssignment")
                        .IsRequired();
                });

            modelBuilder.Entity("MohiuddinCoreMasterDetailCrud.Models.Student", b =>
                {
                    b.Navigation("Enrollments");

                    b.Navigation("StudentDetails")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
