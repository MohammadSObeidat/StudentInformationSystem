namespace SIS.Models
{
    public class InstructorCoursesSectionModel
    {
        public int Id { get; set; }
        public int SectionNumber { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public int StudentCount { get; set; }

    }
}
