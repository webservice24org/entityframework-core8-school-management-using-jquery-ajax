namespace MohiuddinCoreMasterDetailCrud.Models
{
    public class CourseInstructor
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int InstructorID { get; set; }
        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
