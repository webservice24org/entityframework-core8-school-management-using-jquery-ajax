using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = null!;
        [Required]
        [Display(Name = "Select Department")]
        public int DepartmentID { get; set; }
    }

}
