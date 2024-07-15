using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models.ViewModels
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime Dob { get; set; }
        public int CourseId { get; set; }
        public string MobileNo { get; set; }
        public bool IsEnrolled { get; set; }
        public IFormFile ProfileFile { get; set; } 
        public string ImageUrl { get; set; } 
        
    }
}
