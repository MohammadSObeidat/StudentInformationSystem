using System.ComponentModel.DataAnnotations;

namespace SIS.Models
{
    public class SectionModel
    {
        public int Id { get; set; }
        [Required]
        public int SectionNumber { get; set; }
        [Required]
        public int MaxCapacity { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int InstructorId { get; set; }

        public string CourseName { get; set; }
        public string InstructorName { get; set; }
    }
}
