namespace SIS.Models
{
    public class PagedStudentResult
    {
        public List<StudentModel> Students { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
