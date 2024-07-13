using System.ComponentModel.DataAnnotations;
namespace MohiuddinCoreMasterDetailCrud.Models.ViewModels
{
    public class DepartmentViewModel
    {
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        [StringLength(50, ErrorMessage = "Department Name must not exceed 50 characters")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Budget is required")]
        [DataType(DataType.Currency)]
        [Display(Name = "Department Budget")]
        public decimal Budget { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Department Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Administrator is required")]
        [Display(Name = "Select Administrator")]
        public int InstructorID { get; set; }
        public string InstructorName { get; set; }
    }


}
