namespace SIS.Models
{
    public class VmStudent
    {
        public StudentModel Student { get; set; }
        public List<DepartmentModel> Department { get; set; }

        public VmStudent()
        {
            Student = new StudentModel();

            Department = new List<DepartmentModel>();
        }
    }
}
