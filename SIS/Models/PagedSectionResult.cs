namespace SIS.Models
{
    public class PagedSectionResult
    {
        public List<SectionModel> Sections { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
