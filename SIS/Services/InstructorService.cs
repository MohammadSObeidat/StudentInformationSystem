using SIS.Data;
using SIS.Models;

namespace SIS.Services
{
    public class InstructorService : IInstructorService
    {
        SisContext sisContext;
        public InstructorService(SisContext sisContext)
        {
            this.sisContext = sisContext;
        }

        public CreateAccount CreateInstructor(VmInstructor vmInstructor)
        {
            bool studentExists = sisContext.Instructor.Any(s => s.InstructorNumber == vmInstructor.Instructor.InstructorNumber);

            if (studentExists)
            {
                return new CreateAccount
                {
                    Message = "Instructor number already exists. Please enter a unique Instructor number."
                };
            }

            Instructor instructor = new Instructor();

            instructor.InstructorNumber = vmInstructor.Instructor.InstructorNumber;
            instructor.FirstName = vmInstructor.Instructor.FirstName;
            instructor.LastName = vmInstructor.Instructor.LastName;
            instructor.Email = vmInstructor.Instructor.Email;
            instructor.DOB = vmInstructor.Instructor.DOB;
            instructor.Gender = vmInstructor.Instructor.Gender;
            instructor.FileName = vmInstructor.Instructor.FileName;
            instructor.DepartmentId = vmInstructor.Instructor.DepartmentId;

            sisContext.Instructor.Add(instructor);

            sisContext.SaveChanges();

            return new CreateAccount
            {
                Id = instructor.Id,
                Message = "Instructor Details && Account Saved Successfully"
            };
        }

        public PagedInstructorResult GetAllInstructors(int instructorNumber, int page)
        {
            //List<Instructor> instructors = sisContext.Instructor.ToList();

            //List<InstructorModel> instructorsModel = new List<InstructorModel>();

            //foreach (Instructor instructor in instructors)
            //{
            //    InstructorModel instructorModel = new InstructorModel();

            //    instructorModel.Id = instructor.Id;
            //    instructorModel.InstructorNumber = instructor.InstructorNumber;
            //    instructorModel.FirstName = instructor.FirstName;
            //    instructorModel.LastName = instructor.LastName;
            //    instructorModel.Email = instructor.Email;
            //    instructorModel.DOB = instructor.DOB;
            //    instructorModel.Gender = instructor.Gender;
            //    instructorModel.FileName = instructor.FileName;
            //    instructorModel.DepartmentId = instructor.DepartmentId;

            //    instructorsModel.Add(instructorModel);
            //}

            //List<InstructorModel> instructors = sisContext.Instructor.Select(i => new InstructorModel
            //{
            //    Id = i.Id,
            //    InstructorNumber = i.InstructorNumber,
            //    FirstName = i.FirstName,
            //    LastName = i.LastName,
            //    Email = i.Email,
            //    DOB = i.DOB,
            //    Gender = i.Gender,
            //    FileName = i.FileName,
            //    DepartmentName = i.Department.Name

            //}).ToList();

            IQueryable<Instructor> query = sisContext.Instructor;

            // Search 
            if (instructorNumber != 0)
            {
                query = query.Where(i => i.InstructorNumber == instructorNumber);
            }

            // Pagination
            int pageSize = 10;
            int currentPage = page > 0 ? page : 1;
            int skip = (currentPage - 1) * pageSize;

            int totalStudents = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            query = query.Skip(skip).Take(pageSize);


            List<InstructorModel> instructors = query.Select(i => new InstructorModel
            {
                Id = i.Id,
                InstructorNumber = i.InstructorNumber,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                DOB = i.DOB,
                Gender = i.Gender,
                FileName = i.FileName,
                DepartmentName = i.Department.Name

            }).ToList();

            return new PagedInstructorResult
            {
                Instructors = instructors,
                CurrentPage = currentPage,
                TotalPages = totalPages
            };
        }

        public InstructorModel GetInstructorById(int id)
        {
            Instructor instructor = sisContext.Instructor.Find(id);

            InstructorModel instructorModel = new InstructorModel();
            instructorModel.Id = instructor.Id;
            instructorModel.InstructorNumber = instructor.InstructorNumber;
            instructorModel.FirstName = instructor.FirstName;
            instructorModel.LastName = instructor.LastName;
            instructorModel.Email = instructor.Email;
            instructorModel.DOB = instructor.DOB;
            instructorModel.Gender = instructor.Gender;
            instructorModel.FileName = instructor.FileName;
            instructorModel.ExistingImage = instructor.FileName;
            instructorModel.DepartmentId = instructor.DepartmentId;

            return instructorModel;
        }

        public void UpdateInstructor(VmInstructor vmInstructor)
        {
            Instructor instructor = sisContext.Instructor.Find(vmInstructor.Instructor.Id);

            instructor.InstructorNumber = vmInstructor.Instructor.InstructorNumber;
            instructor.FirstName = vmInstructor.Instructor.FirstName;
            instructor.LastName = vmInstructor.Instructor.LastName;
            instructor.Email = vmInstructor.Instructor.Email;
            instructor.DOB = vmInstructor.Instructor.DOB;
            instructor.Gender = vmInstructor.Instructor.Gender;
            instructor.FileName = vmInstructor.Instructor.FileName;
            instructor.DepartmentId = vmInstructor.Instructor.DepartmentId;

            sisContext.SaveChanges();
        }

        public void DeleteInstructor(int id)
        {
            Instructor instructor = sisContext.Instructor.Find(id);

            sisContext.Remove(instructor);

            sisContext.SaveChanges();
        }

        public List<InstructorCoursesModel> GetInstructorCourses(int id)
        {
            List<InstructorCoursesModel> courses = (from s in sisContext.Section
                                                    join c in sisContext.Course on s.CourseId equals c.Id
                                                    where s.InstructorId == id
                                                    select new InstructorCoursesModel
                                                    {
                                                        CourseId = c.Id,
                                                        CourseName = c.Name,
                                                        CourseCount = c.Sections.Count,
                                                        InstructorId = id

                                                    }).ToList();

            return courses;
        }

        public List<InstructorCoursesSectionModel> GetSectionsByInstructorAndCourse(int instructorId, int courseId)
        {
            List<InstructorCoursesSectionModel> sections = sisContext.Section.Where(s => s.InstructorId == instructorId && s.CourseId == courseId)
                                             .Select(s => new InstructorCoursesSectionModel
                                             {
                                                 Id = s.Id,
                                                 SectionNumber = s.SectionNumber,
                                                 CourseId = s.CourseId,
                                                 InstructorId = s.InstructorId,
                                                 StudentCount = sisContext.StudentSection.Count(ss => ss.SectionId == s.Id)
                                             })
                                             .ToList();

            //List<InstructorCoursesSectionModel> sections = (from s in sisContext.Section
            //                                                join ss in sisContext.StudentSection on s.Id equals ss.SectionId
            //                                                where s.InstructorId == instructorId && s.CourseId == courseId
            //                                                group ss by new { s.Id, s.SectionNumber, s.CourseId, s.InstructorId } into g
            //                                                select new InstructorCoursesSectionModel
            //                                                {
            //                                                    Id = g.Key.Id,
            //                                                    SectionNumber = g.Key.SectionNumber,
            //                                                    CourseId = g.Key.CourseId,
            //                                                    InstructorId = g.Key.InstructorId,
            //                                                    StudentCount = g.Count()
            //                                                }
            //                                                ).ToList();

            return sections;
        }

        public List<StudentGradeModel> GetStudentsBySectionId(int instructorId, int courseId, int sectionId)
        {
            //List<StudentModel> students = (from ss in sisContext.StudentSection
            //                               join s in sisContext.Student on ss.StudentId equals s.Id
            //                               join sec in sisContext.Section on ss.SectionId equals sec.Id
            //                               where ss.SectionId == sectionId
            //                                     && sec.CourseId == courseId
            //                                     && sec.InstructorId == instructorId
            //                               select new StudentModel
            //                               {
            //                                   StudentNumber = s.StudentNumber,
            //                                   FirstName = s.FirstName,
            //                                   LastName = s.LastName,
            //                                   DepartmentName = s.Department.Name
            //                               }).ToList();

            List<StudentGradeModel> students = sisContext.StudentSection.Where(ss => ss.SectionId == sectionId
                                                                    && ss.Section.Course.Id == courseId
                                                                    && ss.Section.Instructor.Id == instructorId).Select(ss => new StudentGradeModel
                                                                    {
                                                                        Id = ss.Student.Id,
                                                                        StudentNumber = ss.Student.StudentNumber,
                                                                        FirstName = ss.Student.FirstName,
                                                                        LastName = ss.Student.LastName,
                                                                        DepartmentName = ss.Student.Department.Name,
                                                                        SectionId = sectionId

                                                                    }).ToList();

            return students;
        }

        public void SaveGrades(List<StudentGradeModel> studentsGrades)
        {
            List<Grade> gradesList = new List<Grade>();

            foreach (StudentGradeModel student in studentsGrades)
            {
                Grade grade = new Grade();
                grade.Mark = student.Mark;
                grade.SectionId = student.SectionId;
                grade.StudentId = student.Id;

                gradesList.Add(grade);
            }

            sisContext.Grade.AddRange(gradesList);

            sisContext.SaveChanges();
        }
    }
}
