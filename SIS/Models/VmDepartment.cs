namespace SIS.Models
{
    public class VmDepartment
    {
        public DepartmentModel Department { get; set; }
        public List<DepartmentModel> Departments { get; set; }

        public VmDepartment()
        {
            Department = new DepartmentModel();

            Departments = new List<DepartmentModel>();
        }
    }
}
