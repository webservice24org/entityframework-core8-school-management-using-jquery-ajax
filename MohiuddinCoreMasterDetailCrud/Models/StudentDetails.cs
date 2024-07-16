using System.ComponentModel.DataAnnotations;

namespace MohiuddinCoreMasterDetailCrud.Models
{
    public class StudentDetails
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? GuardianName { get; set; }
        public string? RelationWithGuardian { get; set; }
        public string? GuardianMobile { get; set; }
    }



}
