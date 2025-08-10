using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIS.Models
{
    public class InstructorModel
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 9999999999, ErrorMessage = "Instructor number must be up to 10 digits.")]
        public int InstructorNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public IFormFile? Image { get; set; }
        public string FileName { get; set; }
        public string? ExistingImage { get; set; }
    }
}
