namespace SIS.Models
{
    public class StudentGradeModel
    {
        public int Id { get; set; }
        public int StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DepartmentName { get; set; }
        public int SectionId { get; set; }
        public int Mark { get; set; }
    }
}
