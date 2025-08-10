using Microsoft.EntityFrameworkCore;
using SIS.Data;
using SIS.Models;

namespace SIS.Services
{
    public class CourseService : ICourseService
    {
        SisContext sisContext;
        public CourseService(SisContext sisContext)
        {
            this.sisContext = sisContext;
        }

        public void CreateCourse(VmCourse vmCourse)
        {
            Course course = new Course();
            course.Name = vmCourse.Course.Name;
            course.Description = vmCourse.Course.Description;
            course.DepartmentId = vmCourse.Course.DepartmentId;

            sisContext.Course.Add(course);

            sisContext.SaveChanges();
        }

        public PagedCourseResult GetAllCourses(string name, int page)
        {
            //List<Course> courses = sisContext.Course.ToList();

            //List<CourseModel> courseModels = new List<CourseModel>();

            //foreach (Course course in courses)
            //{
            //    CourseModel courseModel = new CourseModel();
            //    courseModel.Id = course.Id;
            //    courseModel.Name = course.Name;
            //    courseModel.Description = course.Description;

            //    courseModels.Add(courseModel);
            //}

            //List<CourseModel> courses = sisContext.Course.Select(c => new CourseModel
            //{
            //    Id = c.Id,
            //    Name = c.Name,
            //    Description = c.Description,
            //    DepartmentName = c.Department.Name,

            //}).ToList();

            IQueryable<Course> query = sisContext.Course;

            // Search
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            // Pagination
            int pageSize = 10;
            int currentPage = page > 0 ? page : 1;
            int skip = (currentPage - 1) * pageSize;

            int totalStudents = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

            query = query.Skip(skip).Take(pageSize);

            List<CourseModel> courses = query.Select(c => new CourseModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                DepartmentName = c.Department.Name,

            }).ToList();

            return new PagedCourseResult
            {
                Courses = courses,
                TotalPages = totalPages,
                CurrentPage = currentPage,
            };
        }

        public CourseModel GetCourseById(int id)
        {
            Course course = sisContext.Course.Find(id);

            CourseModel courseModel = new CourseModel();
            courseModel.Id = course.Id;
            courseModel.Name = course.Name;
            courseModel.Description = course.Description;
            courseModel.DepartmentId = course.DepartmentId;

            return courseModel;
        }

        public void UpdateCourse(VmCourse vmCourse)
        {
            Course course = sisContext.Course.Find(vmCourse.Course.Id);

            course.Name = vmCourse.Course.Name;
            course.Description = vmCourse.Course.Description;
            course.DepartmentId = vmCourse.Course.DepartmentId;

            sisContext.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            Course course = sisContext.Course.Find(id);

            sisContext.Course.Remove(course);

            sisContext.SaveChanges();
        }

        public List<CourseModel> GetCoursesByDepartmentId(int id)
        {
            List<Course> courses = sisContext.Course.Include(c => c.Department).Include(c => c.Sections).Where(c => c.DepartmentId == id).ToList();

            List<CourseModel> courseModels = new List<CourseModel>();

            foreach (Course course in courses)
            {
                CourseModel courseModel = new CourseModel();
                courseModel.Id = course.Id;
                courseModel.Name = course.Name;
                courseModel.Description = course.Description;
                courseModel.DepartmentName = course.Department?.Name;
                courseModel.SectionCount = course.Sections?.Count ?? 0;

                courseModels.Add(courseModel);
            }

            return courseModels;
        }
    }
}
