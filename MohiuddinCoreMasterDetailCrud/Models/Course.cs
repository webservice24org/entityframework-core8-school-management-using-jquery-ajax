using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models
{

    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; } = null!;

        [Required]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<CourseInstructor> CourseInstructor { get; set; } = new List<CourseInstructor>();
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>(); 
    }

}
