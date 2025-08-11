using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIS.Data;
using SIS.Models;
using SIS.Services;
using System.Data;

namespace SIS.Controllers
{
    [Authorize(Roles = "Manager, Student")]
    public class StudentController : Controller
    {
        IDepartmentService service;
        IStudentService studentService;
        IAccountService accountService;
        public StudentController(IDepartmentService service, IStudentService studentService, IAccountService accountService)
        {
            this.service = service;
            this.studentService = studentService;
            this.accountService = accountService;
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Index(int studentNumber, int page)
        {
            PagedStudentResult students = studentService.GetAllStudents(studentNumber, page);

            if (!students.Students.Any())
            {
                ViewBag.Message = "No student found.";
            }

            return View("Index", students);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            VmStudent vmStudent = new VmStudent();

            vmStudent.Department = service.GetAllDepartment("");

            ViewData["isEdit"] = false;

            return View("Create", vmStudent);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Save(VmStudent vmStudent)
        {
            if (vmStudent.Student.Image != null && vmStudent.Student.Image.Length > 0)
            {
                // save image
                string imageName = Guid.NewGuid().ToString() + "." + vmStudent.Student.Image.FileName.Split('.')[1];
                string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", imageName);
                vmStudent.Student.Image.CopyTo(new FileStream(path, FileMode.Create));
                vmStudent.Student.FileName = imageName;
            }
            // 1- Create Student
            CreateAccount result = studentService.CreateStudent(vmStudent);

            vmStudent.Student.Id = result.Id;

            // 2- Create Account
            var accountResult = await accountService.CreateStudentAccount(vmStudent, "Student");

            if (!accountResult.Succeeded)
            {
                studentService.DeleteStudent(result.Id);

                TempData["error"] = "Failed to create student account";

                return RedirectToAction("Index");
            }

            TempData["message"] = result.Message;

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Edit(int id)
        {
            VmStudent vmStudent = new VmStudent();

            vmStudent.Student = studentService.GetStudentById(id);

            vmStudent.Department = service.GetAllDepartment("");

            ViewData["isEdit"] = true;

            return View("Create", vmStudent);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Update(VmStudent vmStudent)
        {
            if (vmStudent.Student.Image != null && vmStudent.Student.Image.Length > 0)
            {
                // save image
                string imageName = Guid.NewGuid().ToString() + "." + vmStudent.Student.Image.FileName.Split('.')[1];
                string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", imageName);
                vmStudent.Student.Image.CopyTo(new FileStream(path, FileMode.Create));
                vmStudent.Student.FileName = imageName;
            }
            else
            {
                vmStudent.Student.FileName = vmStudent.Student.ExistingImage;
            }


            studentService.UpdateStudent(vmStudent);

            TempData["message"] = "Student Updated Successfully";

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            Student student = studentService.DeleteStudent(id);

            await accountService.DeleteAccount(student.UserId);

            TempData["message"] = "Student Deleted Successfully";

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        public IActionResult CourseRegister(int studentId, int sectionId)
        {
            studentService.CourseRegister(studentId, sectionId);

            TempData["message"] = "Course registration completed successfully";

            return RedirectToAction("getRegisteredCourses", new { studentId = studentId });
        }

        [Authorize(Roles = "Student")]
        public IActionResult getRegisteredCourses(int studentId)
        {
            List<RegisteredCoursesModel> courses = studentService.getRegisteredCourses(studentId);

            if (!courses.Any())
            {
                ViewBag.Message = "No courses found.";
            }

            return View("getRegisteredCourses", courses);
        }

        [Authorize(Roles = "Student")]
        public IActionResult RemoveStudentSection(int studentId, int sectionId)
        {
            studentService.RemoveStudentSection(studentId, sectionId);

            TempData["message"] = "Course deleted successfully";

            return RedirectToAction("getRegisteredCourses", new { studentId = studentId });
        }
    }
}
