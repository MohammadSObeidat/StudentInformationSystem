using System.ComponentModel.DataAnnotations.Schema;

namespace SIS.Data
{
    [Table("Sections")]
    public class Section
    {
        public int Id { get; set; }

        public int SectionNumber { get; set; }
        public int MaxCapacity { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        public List<StudentSection> StudentSection { get; set; }
        public List<Grade> Grades { get; set; }
    }
}
