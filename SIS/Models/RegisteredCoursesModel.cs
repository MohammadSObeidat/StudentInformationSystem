namespace SIS.Models
{
    public class RegisteredCoursesModel
    {
        public int SectionId { get; set; }
        public int SectionNumber { get; set; }
        public string CourseName { get; set; }
        public string InstructorName { get; set; }
        public int Mark { get; set; }
    }
}
