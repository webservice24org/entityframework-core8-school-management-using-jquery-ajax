
using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models.ViewModels
{
    public class ModuleViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Select Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<ModuleViewModel> Modules { get; set; } = new List<ModuleViewModel>();
    }



}
