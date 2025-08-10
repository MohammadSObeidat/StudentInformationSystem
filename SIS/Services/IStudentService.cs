using SIS.Models;

namespace SIS.Services
{
    public interface IStudentService
    {
        CreateAccount CreateStudent(VmStudent vmStudent);
        PagedStudentResult GetAllStudents(int studentNumber, int page);
        StudentModel GetStudentById(int id);
        void UpdateStudent(VmStudent vmStudent);
        void DeleteStudent(int id);
        void CourseRegister(int studentId, int sectionId);
        List<RegisteredCoursesModel> getRegisteredCourses(int studentId);
        void RemoveStudentSection(int studentId, int sectionId);
    }
}