using Microsoft.EntityFrameworkCore;
using SIS.Data;
using SIS.Models;

namespace SIS.Services
{
    public class DepartmentService : IDepartmentService
    {
        SisContext sisContext;
        public DepartmentService(SisContext sisContext)
        {
            this.sisContext = sisContext;
        }

        public void CreateDepartment(string name)
        {
            Department department = new Department();
            department.Name = name;

            sisContext.Department.Add(department);

            sisContext.SaveChanges();
        }

        public List<DepartmentModel> GetAllDepartment(string? name)
        {
            //List<Department> departments = sisContext.Department.ToList();

            //List<DepartmentModel> departmentsModel = new List<DepartmentModel>();

            //foreach (Department department in departments)
            //{
            //    DepartmentModel departmentModel = new DepartmentModel();
            //    departmentModel.Id = department.Id;
            //    departmentModel.Name = department.Name;

            //    departmentsModel.Add(departmentModel);
            //}

            //List<DepartmentModel> departments = sisContext.Department.Select(
            //    d => new DepartmentModel
            //    {
            //        Id = d.Id,
            //        Name = d.Name,
            //        StudentCount = d.Students.Count(s => s.DepartmentId == d.Id)
            //    }
            //    ).ToList();


            IQueryable<Department> query = sisContext.Department;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }

            List<DepartmentModel> departments = query.Select(d => new DepartmentModel
            {
                Id = d.Id,
                Name = d.Name,
                StudentCount = d.Students.Count(s => s.DepartmentId == d.Id),
                InstructorCount = d.Instructors.Count(i => i.DepartmentId == d.Id)

            }).ToList();

            return departments;
        }

        public DepartmentModel GetDepartmentById(int departmentId)
        {
            Department department = sisContext.Department.Find(departmentId);

            DepartmentModel departmentModel = new DepartmentModel();
            departmentModel.Id = department.Id;
            departmentModel.Name = department.Name;

            return departmentModel;
        }

        public void UpdateDepartment(VmDepartment vmDepartment)
        {
            Department department = sisContext.Department.Find(vmDepartment.Department.Id);

            department.Name = vmDepartment.Department.Name;

            sisContext.SaveChanges();
        }

        public void DeleteDepartment(int departmentId)
        {
            Department department = sisContext.Department.Find(departmentId);
            sisContext.Department.Remove(department);

            sisContext.SaveChanges();
        }

        public DashboardCounts GetAllCount()
        {
            return new DashboardCounts
            {
                Departments = sisContext.Department.Count(),
                Students = sisContext.Student.Count(),
                Instructors = sisContext.Instructor.Count(),
                Courses = sisContext.Course.Count(),
                Sections = sisContext.Section.Count()
            };
        }
    }
}
