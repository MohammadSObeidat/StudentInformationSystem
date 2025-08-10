namespace SIS.Models
{
    public class InstructorCoursesModel
    {
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public  string CourseName { get; set; }
        public int CourseCount { get; set; }
    }
}
