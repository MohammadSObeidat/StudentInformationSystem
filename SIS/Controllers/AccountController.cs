using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIS.Data;
using SIS.Models;
using SIS.Services;

namespace SIS.Controllers
{
    public class AccountController : Controller
    {
        IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            var result = await accountService.SignIn(signInModel);

            if (result.Succeeded)
            {
                TempData["message"] = "Welcome back! You are now signed in.";

                return RedirectToAction("Index", "Dashboard");
            }

            TempData["error"] = "Invalid email or password.";

            return View("SignIn");
        }

        public async Task<IActionResult> Logout()
        {
            await accountService.Logout();

            return RedirectToAction("SignIn", "Account");
        }

        public IActionResult Profile()
        {
            return View();
        }

        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (!ModelState.IsValid)
                return View(changePasswordModel);

            var result = await accountService.ChangePassword(changePasswordModel);

            if (result.Succeeded)
            {
                TempData["message"] = "Password changed successfully.";

                return RedirectToAction("Profile");
            }

            return View(changePasswordModel);
        }
    }
}
