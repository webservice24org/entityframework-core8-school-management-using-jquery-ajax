using Microsoft.AspNetCore.Http;
using MohiuddinCoreMasterDetailCrud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MohiuddinCoreMasterDetailCrud.Models.ViewModels
{
    public class InstructorViewModel
    {

        public int InstructorID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }

        public int? InstructorDetailsID { get; set; }

        [Display(Name = "Present Address")]
        public string PresentAddress { get; set; }

        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? Dob { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Instructor's Salary")]
        public decimal? Salary { get; set; }

        [StringLength(255)]
    
        public string InstructorPicture { get; set; }

        [Display(Name = "Instructor's Picture")]
        public IFormFile InstructorProfile { get; set; }

        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }

        public OfficeAssignment OfficeAssignment { get; set; }

        public IEnumerable<int> SelectedCourseIDs { get; set; }
        public IEnumerable<Course> AllCourses { get; set; }
    }
}
