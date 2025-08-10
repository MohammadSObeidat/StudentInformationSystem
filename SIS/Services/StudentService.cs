using SIS.Data;
using SIS.Models;

namespace SIS.Services
{
    public class StudentService : IStudentService
    {
        SisContext sisContext;
        public StudentService(SisContext sisContext)
        {
            this.sisContext = sisContext;
        }

        public CreateAccount CreateStudent(VmStudent vmStudent)
        {
            bool studentExists = sisContext.Student.Any(s => s.StudentNumber == vmStudent.Student.StudentNumber);

            if (studentExists)
            {
                return new CreateAccount
                {
                    Message = "Student number already exists. Please enter a unique student number."
                };

            }

            Student student = new Student();

            student.FirstName = vmStudent.Student.FirstName;
            student.LastName = vmStudent.Student.LastName;
            student.Email = vmStudent.Student.Email;
            student.DOB = vmStudent.Student.DOB;
            student.Gender = vmStudent.Student.Gender;
            student.DepartmentId = vmStudent.Student.DepartmentId;
            student.FileName = vmStudent.Student.FileName;
            student.StudentNumber = vmStudent.Student.StudentNumber;

            sisContext.Student.Add(student);

            sisContext.SaveChanges();

            return new CreateAccount
            {
                Id = student.Id,
                Message = "Student Details && Account Saved Successfully"
            };
        }

        public PagedStudentResult GetAllStudents(int studentNumber, int page)
        {
            //List<StudentModel> students = sisContext.Student.Select(s => new StudentModel
            //{
            //    Id = s.Id,
            //    StudentNumber = s.StudentNumber,
            //    FirstName = s.FirstName,
            //    LastName = s.LastName,
            //    Email = s.Email,
            //    DOB = s.DOB,
            //    Gender = s.Gender,
            //    DepartmentName = s.Department.Name,
            //    FileName = s.FileName,

            //}).ToList();

            IQueryable<Student> query = sisContext.Student;

            // Search 
            if (studentNumber != 0)
            {
                query = query.Where(s => s.StudentNumber == studentNumber);
            }

            // Pagination
            int pageSize = 10;
            int currentPage = page > 0 ? page : 1;
            int skip = (currentPage - 1) * pageSize;

            int totalStudents = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            query = query.Skip(skip).Take(pageSize);


            List<StudentModel> students = query.Select(s => new StudentModel
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                DOB = s.DOB,
                Gender = s.Gender,
                DepartmentName = s.Department.Name,
                FileName = s.FileName,

            }).ToList();

            return new PagedStudentResult
            {
                Students = students,
                TotalPages = totalPages,
                CurrentPage = currentPage
            };
        }

        public StudentModel GetStudentById(int id)
        {
            Student student = sisContext.Student.Find(id);

            StudentModel studentModel = new StudentModel();
            studentModel.Id = student.Id;
            studentModel.StudentNumber = student.StudentNumber;
            studentModel.FirstName = student.FirstName;
            studentModel.LastName = student.LastName;
            studentModel.Email = student.Email;
            studentModel.DOB = student.DOB;
            studentModel.Gender = student.Gender;
            studentModel.DepartmentId = student.DepartmentId;
            studentModel.FileName = student.FileName;
            studentModel.ExistingImage = student.FileName;

            return studentModel;
        }

        public void UpdateStudent(VmStudent vmStudent)
        {
            Student student = sisContext.Student.Find(vmStudent.Student.Id);

            student.StudentNumber = vmStudent.Student.StudentNumber;
            student.FirstName = vmStudent.Student.FirstName;
            student.LastName = vmStudent.Student.LastName;
            student.Email = vmStudent.Student.Email;
            student.DOB = vmStudent.Student.DOB;
            student.Gender = vmStudent.Student.Gender;
            student.DepartmentId = vmStudent.Student.DepartmentId;
            student.FileName = vmStudent.Student.FileName;

            sisContext.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            Student student = sisContext.Student.Find(id);
            sisContext.Remove(student);

            sisContext.SaveChanges();
        }

        public void CourseRegister(int studentId, int sectionId)
        {
            StudentSection studentSection = new StudentSection();
            studentSection.StudentId = studentId;
            studentSection.SectionId = sectionId;

            sisContext.StudentSection.Add(studentSection);

            sisContext.SaveChanges();
        }

        public List<RegisteredCoursesModel> getRegisteredCourses(int studentId)
        {
            var courses = sisContext.StudentSection.Where(ss => ss.StudentId == studentId)
                                                   .Select(ss => new RegisteredCoursesModel
                                                   {
                                                       SectionId = ss.SectionId,
                                                       SectionNumber = ss.Section.SectionNumber,
                                                       CourseName = ss.Section.Course.Name,
                                                       InstructorName = ss.Section.Instructor.FirstName + " " + ss.Section.Instructor.LastName,
                                                       Mark = ss.Section.Grades
                                                                    .Where(g => g.StudentId == studentId)
                                                                    .Select(g => g.Mark)
                                                                    .FirstOrDefault()
                                                   }).ToList();
            return courses;
        }

        public void RemoveStudentSection(int studentId, int sectionId)
        {
            StudentSection course = sisContext.StudentSection.Find(studentId, sectionId);

            sisContext.StudentSection.Remove(course);

            sisContext.SaveChanges();
        }
    }
}
