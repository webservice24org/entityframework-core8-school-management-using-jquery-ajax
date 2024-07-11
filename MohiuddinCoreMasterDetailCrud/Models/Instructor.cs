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
        [StringLength(100)]
        public string InstructorPicture { get; set; }
        public virtual ICollection<CourseInstructor> CourseInstructors { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }


}
