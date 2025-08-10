namespace SIS.Models
{
    public class PagedInstructorResult
    {
        public List<InstructorModel> Instructors { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
