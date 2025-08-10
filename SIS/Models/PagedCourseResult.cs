namespace SIS.Models
{
    public class PagedCourseResult
    {
        public List<CourseModel> Courses { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
