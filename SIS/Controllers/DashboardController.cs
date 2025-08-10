using Microsoft.AspNetCore.Mvc;
using SIS.Models;
using SIS.Services;

namespace SIS.Controllers
{
    public class DashboardController : Controller
    {
        IDepartmentService service;
        public DashboardController(IDepartmentService service)
        {
            this.service = service;
        }
        public IActionResult Index()
        {
            DashboardCounts counts = service.GetAllCount();

            return View("Index", counts);
        }
    }
}
