namespace SIS.Models
{
    public class VmCourse
    {
        public CourseModel Course { get; set; }
        public List<DepartmentModel> Departments { get; set; }

        public VmCourse()
        {
            Course = new CourseModel();

            Departments = new List<DepartmentModel>();
        }
    }
}
