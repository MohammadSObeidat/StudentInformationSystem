using System.ComponentModel.DataAnnotations;

namespace SIS.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int StudentCount { get; set; }
        public int InstructorCount { get; set; }
    }
}
