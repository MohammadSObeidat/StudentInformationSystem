using SIS.Data;
using SIS.Models;

namespace SIS.Services
{
    public interface IInstructorService
    {
        CreateAccount CreateInstructor(VmInstructor vmInstructor);
        PagedInstructorResult GetAllInstructors(int instructorNumber, int page);
        InstructorModel GetInstructorById(int id);
        void UpdateInstructor(VmInstructor vmInstructor);
        Instructor DeleteInstructor(int id);
        List<InstructorCoursesModel> GetInstructorCourses(int id);
        List<InstructorCoursesSectionModel> GetSectionsByInstructorAndCourse(int instructorId, int courseId);
        List<StudentGradeModel> GetStudentsBySectionId(int instructorId, int courseId, int sectionId);
        void SaveGrades(List<StudentGradeModel> studentsGrades);
    }
}