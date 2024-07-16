using System;
using System.Collections.Generic;

namespace MohiuddinCoreMasterDetailCrud.Models 
{

    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime Dob { get; set; }
        public string Mobile { get; set; }
        public string ImageUrl { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int? StudentDetailsId { get; set; }
        public virtual StudentDetails StudentDetails { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }



}