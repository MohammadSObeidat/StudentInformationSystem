using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIS.Data;
using SIS.Models;
using SIS.Services;
using static System.Collections.Specialized.BitVector32;

namespace SIS.Controllers
{
    [Authorize(Roles = "Manager, Instructor")]
    public class InstructorController : Controller
    {
        IDepartmentService service;
        IInstructorService instructorService;
        IAccountService accountService;
        public InstructorController(IDepartmentService service,
            IInstructorService instructorService,
            IAccountService accountService)
        {
            this.service = service;
            this.instructorService = instructorService;
            this.accountService = accountService;
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Index(int instructorNumber, int page)
        {
            PagedInstructorResult instructors = instructorService.GetAllInstructors(instructorNumber, page);

            if (!instructors.Instructors.Any())
            {
                ViewBag.Message = "No Instructor found.";
            }

            return View("Index", instructors);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            VmInstructor vmInstructor = new VmInstructor();
            vmInstructor.Department = service.GetAllDepartment("");

            ViewData["isEdit"] = false;

            return View("Create", vmInstructor);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Save(VmInstructor vmInstructor)
        {
            if (vmInstructor.Instructor.Image != null && vmInstructor.Instructor.Image.Length > 0)
            {
                // save image
                string imageName = Guid.NewGuid().ToString() + "." + vmInstructor.Instructor.Image.FileName.Split('.')[1];
                string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", imageName);
                vmInstructor.Instructor.Image.CopyTo(new FileStream(path, FileMode.Create));
                vmInstructor.Instructor.FileName = imageName;
            }

            // 1- Create Instructor
            CreateAccount result = instructorService.CreateInstructor(vmInstructor);

            vmInstructor.Instructor.Id = result.Id;

            // 2- Create Account
            var accountResult = await accountService.CreateInstructorAccount(vmInstructor, "Instructor");

            if (!accountResult.Succeeded)
            {
                instructorService.DeleteInstructor(result.Id);

                TempData["error"] = "Failed to create instructor account";

                return RedirectToAction("Index");
            }

            TempData["message"] = result.Message;

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Edit(int id)
        {
            VmInstructor vmInstructor = new VmInstructor();

            vmInstructor.Instructor = instructorService.GetInstructorById(id);
            vmInstructor.Department = service.GetAllDepartment("");

            ViewData["isEdit"] = true;

            return View("Create", vmInstructor);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Update(VmInstructor vmInstructor)
        {
            if (vmInstructor.Instructor.Image != null && vmInstructor.Instructor.Image.Length > 0)
            {
                // save image
                string imageName = Guid.NewGuid().ToString() + "." + vmInstructor.Instructor.Image.FileName.Split('.')[1];
                string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", imageName);
                vmInstructor.Instructor.Image.CopyTo(new FileStream(path, FileMode.Create));
                vmInstructor.Instructor.FileName = imageName;
            }
            else
            {
                vmInstructor.Instructor.FileName = vmInstructor.Instructor.ExistingImage;
            }

            instructorService.UpdateInstructor(vmInstructor);

            TempData["message"] = "Instructor Updated Successfully";

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Delete(int id)
        {
            instructorService.DeleteInstructor(id);

            TempData["message"] = "Instructor Deleted Successfully";

            return RedirectToAction("Index");
        }

        public IActionResult GetCoursesByInstructorId(int id)
        {
            List<InstructorCoursesModel> courses = instructorService.GetInstructorCourses(id);

            if (!courses.Any())
            {
                ViewBag.Message = "No courses found.";
            }

            return View("GetCoursesByInstructorId", courses);
        }

        public IActionResult GetSectionsByInstructorAndCourse(int instructorId, int courseId)
        {
            List<InstructorCoursesSectionModel> sections = instructorService.GetSectionsByInstructorAndCourse(instructorId, courseId);

            if (!sections.Any())
            {
                ViewBag.Message = "No sections found.";
            }

            return View("GetSectionsByInstructorAndCourse", sections);
        }

        public IActionResult GetStudentsBySectionId(int instructorId, int courseId, int sectionId)
        {
            List<StudentGradeModel> students = instructorService.GetStudentsBySectionId(instructorId, courseId, sectionId);

            if (!students.Any())
            {
                ViewBag.Message = "No students found.";
            }

            return View("GetStudentsBySectionId", students);
        }

        public IActionResult SaveGrades(List<StudentGradeModel> studentsGrades)
        {
            instructorService.SaveGrades(studentsGrades);

            return RedirectToAction("Dashboard");
        }
    }
}
