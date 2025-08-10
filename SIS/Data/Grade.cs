using System.ComponentModel.DataAnnotations.Schema;

namespace SIS.Data
{
    [Table("Grade")]
    public class Grade
    {
        public int Id { get; set; }
        public int Mark { get; set; }

        [ForeignKey("Section")]
        public int SectionId { get; set; }
        public Section Section { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
