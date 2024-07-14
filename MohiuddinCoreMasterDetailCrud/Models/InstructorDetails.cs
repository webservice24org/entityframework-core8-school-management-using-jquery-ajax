using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models
{
    public class InstructorDetails
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }

        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }

        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Column(TypeName = "money")]
        public decimal Salary { get; set; }

        [StringLength(255)]
        public string InstructorPicture { get; set; }

        public virtual Instructor Instructor { get; set; } 
    }

}
