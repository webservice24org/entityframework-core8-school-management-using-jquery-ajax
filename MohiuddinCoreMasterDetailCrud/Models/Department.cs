using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(50)]
        public string DepartmentName { get; set; }

        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public int InstructorID { get; set; }

        // Navigation property to Instructor
        [ForeignKey("InstructorID")]
        public virtual Instructor Administrator { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }


}
