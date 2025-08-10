using System.ComponentModel.DataAnnotations.Schema;

namespace SIS.Data
{
    [Table("Instructors")]
    public class Instructor
    {
        public int Id { get; set; }
        public int InstructorNumber { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string FileName { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<Section> Sections { get; set; }
    }
}
