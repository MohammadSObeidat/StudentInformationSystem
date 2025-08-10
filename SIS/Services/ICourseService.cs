using SIS.Models;

namespace SIS.Services
{
    public interface ICourseService
    {
        void CreateCourse(VmCourse vmCourse);
        PagedCourseResult GetAllCourses(string name, int page);
        CourseModel GetCourseById(int id);
        void UpdateCourse(VmCourse vmCourse);
        void DeleteCourse(int id);
        List<CourseModel> GetCoursesByDepartmentId(int id);
    }
}