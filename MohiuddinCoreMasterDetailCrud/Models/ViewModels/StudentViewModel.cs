using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models.ViewModels
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class StudentViewModel
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Student Name is required")]
        [Display(Name ="Student Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Select Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }

        public int? StudentDetailsId { get; set; }
        [Display(Name ="Present Address")]
        public string PresentAddress { get; set; }
        
        [Display(Name ="Permanent Address")]
        public string PermanentAddress { get; set; }

        [Display(Name ="Guardian Name")]
        public string GuardianName { get; set; }

        [Display(Name ="Relation With Guardian")]
        public string RelationWithGuardian { get; set; }

        [Display(Name = "Guardian's Mobile")]
        public string GuardianMobile { get; set; }

        public List<int> EnrollmentIds { get; set; } = new List<int>();

        [Display(Name = "Profile Picture")]
        public IFormFile ProfileFile { get; set; }

        public string ImageUrl { get; set; }
    }

}
