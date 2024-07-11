using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models
{

    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<CourseInstructor> CourseInstructors { get; set; }
    }
}
