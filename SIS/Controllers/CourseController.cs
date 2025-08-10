using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIS.Data;
using SIS.Models;
using SIS.Services;

namespace SIS.Controllers
{
    [Authorize(Roles = "Manager, Student")]
    public class CourseController : Controller
    {
        IDepartmentService service;
        ICourseService courseService;
        public CourseController(IDepartmentService service, ICourseService courseService)
        {
            this.service = service;
            this.courseService = courseService;
        }

        public IActionResult Index(string name, int page)
        {
            PagedCourseResult courses = courseService.GetAllCourses(name, page);

            if (!courses.Courses.Any())
            {
                ViewBag.Message = "No course found.";
            }

            return View("Index", courses);
        }

        public IActionResult Create()
        {
            VmCourse vmCourse = new VmCourse();

            vmCourse.Departments = service.GetAllDepartment("");

            ViewData["isEdit"] = false;

            return View("Create", vmCourse);
        }

        public IActionResult Save(VmCourse vmCourse)
        {
            courseService.CreateCourse(vmCourse);

            TempData["message"] = "Course has been saved successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            VmCourse vmCourse = new VmCourse();

            vmCourse.Course = courseService.GetCourseById(id);
            vmCourse.Departments = service.GetAllDepartment("");

            ViewData["isEdit"] = true;

            return View("Create", vmCourse);
        }

        public IActionResult Update(VmCourse vmCourse)
        {
            courseService.UpdateCourse(vmCourse);

            TempData["message"] = "Course Updated successfully.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            courseService.DeleteCourse(id);

            TempData["message"] = "Course Deleted successfully.";

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        public IActionResult GetCoursesByDepartmentId(int id)
        {
            List<CourseModel> courses = courseService.GetCoursesByDepartmentId(id);

            if (!courses.Any())
            {
                ViewBag.Message = "No courses found.";
            }

            return View("GetCoursesByDepartmentId", courses);
        }
    }
}
