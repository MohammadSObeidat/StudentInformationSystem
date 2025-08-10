using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIS.Data;
using SIS.Models;
using SIS.Services;

namespace SIS.Controllers
{
    [Authorize(Roles = "Manager, Instructor, Student")]
    public class SectionController : Controller
    {
        ICourseService courseService;
        IInstructorService instructorService;
        ISectionService sectionService;
        public SectionController(ICourseService courseService, IInstructorService instructorService, ISectionService sectionService)
        {
            this.courseService = courseService;
            this.instructorService = instructorService;
            this.sectionService = sectionService;
        }

        public IActionResult Index(string courseName, int page)
        {
            PagedSectionResult sections = sectionService.GetAllSections(courseName, page);

            if (!sections.Sections.Any())
            {
                ViewBag.Message = "No section found.";
            }

            return View("Index", sections);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            VmSection vmSection = new VmSection();

            vmSection.Course = courseService.GetAllCourses("", 0);
            vmSection.Instructor = instructorService.GetAllInstructors(0, 0);

            ViewData["isEdit"] = false;

            return View("Create", vmSection);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Save(VmSection vmSection)
        {
            sectionService.CreateSection(vmSection);

            TempData["message"] = "Section has been saved successfully.";

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Edit(int id)
        {
            VmSection vmSection = new VmSection();

            vmSection.Section = sectionService.GetSectionById(id);
            vmSection.Course = courseService.GetAllCourses("", 0);
            vmSection.Instructor = instructorService.GetAllInstructors(0, 0);

            ViewData["isEdit"] = true;

            return View("Create", vmSection);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Update(VmSection vmSection)
        {
            sectionService.UpdateSection(vmSection);

            TempData["message"] = "Section has been updated successfully.";

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Delete(int id)
        {
            sectionService.DeleteSection(id);

            TempData["message"] = "Section has been deleted successfully.";

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        public IActionResult GetSectionsByCourseId(int id)
        {
            List<SectionModel> sections = sectionService.GetSectionsByCourseId(id);

            return View("GetSectionsByCourseId", sections);
        }
    }
}
