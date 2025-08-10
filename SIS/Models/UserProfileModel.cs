namespace SIS.Models
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string FileName { get; set; }
    }
}
