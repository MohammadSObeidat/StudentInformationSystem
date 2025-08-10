using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIS.Data;
using SIS.Models;
using SIS.Services;

namespace SIS.Controllers
{
    [Authorize(Roles = "Manager")]
    public class DepartmentController : Controller
    {
        IDepartmentService service;
        public DepartmentController(IDepartmentService service)
        {
            this.service = service;
        }

        public IActionResult Index(string name)
        {
            VmDepartment vmDepartment = new VmDepartment();

            vmDepartment.Departments = service.GetAllDepartment(name);

            if (!vmDepartment.Departments.Any())
            {
                ViewBag.Message = "No departments found.";
            }

            return View("Index", vmDepartment);
        }

        public IActionResult Save()
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            string name = Request.Form["Department.Name"];


            service.CreateDepartment(name);

            TempData["message"] = "Department Details Saved Successfully";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            VmDepartment vmDepartment = new VmDepartment();

            vmDepartment.Departments = service.GetAllDepartment("");

            vmDepartment.Department = service.GetDepartmentById(id);

            return View("Index", vmDepartment);
        }

        public IActionResult Update(VmDepartment vmDepartment)
        {
            service.UpdateDepartment(vmDepartment);

            TempData["message"] = "Department Updated Successfully";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            service.DeleteDepartment(id);

            TempData["message"] = "Department Deleted Successfully";

            return RedirectToAction("Index");
        }
    }
}
