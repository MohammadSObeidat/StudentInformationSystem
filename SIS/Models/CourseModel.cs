using System.ComponentModel.DataAnnotations;

namespace SIS.Models
{
    public class CourseModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int SectionCount { get; set; }
    }
}
