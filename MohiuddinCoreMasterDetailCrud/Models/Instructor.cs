using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models
{
    public class Instructor
    {
        [Key]
        public int InstructorID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        [Required]
        [StringLength(15)]
        public string Mobile { get; set; } // Added Mobile property

        public virtual InstructorDetails InstructorDetails { get; set; } // Navigation property to InstructorDetails

        public virtual ICollection<CourseInstructor> CourseInstructor { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }




}
