using System.ComponentModel.DataAnnotations.Schema;

namespace SIS.Data
{
    [Table("StudentSections")]
    public class StudentSection
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Section")]
        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
