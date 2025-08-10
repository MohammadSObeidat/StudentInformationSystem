using SIS.Models;

namespace SIS.Services
{
    public interface IDepartmentService
    {
        void CreateDepartment(string name);
        void DeleteDepartment(int departmentId);
        List<DepartmentModel> GetAllDepartment(string? name);
        DepartmentModel GetDepartmentById(int departmentId);
        void UpdateDepartment(VmDepartment vmDepartment);
        public DashboardCounts GetAllCount();
    }
}