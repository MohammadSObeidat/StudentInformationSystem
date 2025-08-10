using System.ComponentModel.DataAnnotations.Schema;

namespace SIS.Data
{
    [Table("Courses")]
    public class Course
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public List<Section> Sections { get; set; }
    }
}
