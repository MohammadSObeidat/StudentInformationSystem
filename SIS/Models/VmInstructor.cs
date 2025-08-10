namespace SIS.Models
{
    public class VmInstructor
    {
        public InstructorModel Instructor { get; set; }
        public List<DepartmentModel> Department { get; set; }

        public VmInstructor()
        {
            Instructor = new InstructorModel();

            Department = new List<DepartmentModel>();
        }
    }
}
