using System.ComponentModel.DataAnnotations.Schema;

namespace SIS.Data
{
    [Table("Departments")]
    public class Department
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Name { get; set; }
        public List<Student> Students { get; set; }
        public List<Instructor> Instructors { get; set; }
        public List<Course> Courses { get; set; }
    }
}
