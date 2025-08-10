using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIS.Data
{
    public class ApplicationUser : IdentityUser
    {
        public Student Student { get; set; }
        public Instructor Instructor { get; set; }
    }
}
