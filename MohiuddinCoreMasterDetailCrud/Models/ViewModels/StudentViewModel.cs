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
        public IFormFile ProfileFile { get; set; } // For file upload
        public string ImageUrl { get; set; } // To store the URL of the existing image
        public IList<ModuleViewModel> Modules { get; set; } = new List<ModuleViewModel>(); // Ensure this is initialized
    }
}
