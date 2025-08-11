using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIS.Data;
using SIS.Models;

namespace SIS.Services
{
    public class AccountService : IAccountService
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        SisContext sisContext;
        IHttpContextAccessor httpContextAccessor;

        public AccountService(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              SisContext sisContext,
                              IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.sisContext = sisContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> CreateStudentAccount(VmStudent vmStudent, string role)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = vmStudent.Student.Email;
            user.Email = vmStudent.Student.Email;

            var creationResult = await userManager.CreateAsync(user, vmStudent.Student.Password);

            if (creationResult.Succeeded)
            {
                var student = sisContext.Student.FirstOrDefault(s => s.Id == vmStudent.Student.Id);
                if (student != null)
                {
                    student.UserId = user.Id;
                    sisContext.SaveChanges();
                }

                var roleResult = await userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    await userManager.DeleteAsync(user);
                }
            }

            return creationResult;
        }

        public async Task<IdentityResult> CreateInstructorAccount(VmInstructor vmInstructor, string role)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = vmInstructor.Instructor.Email;
            user.Email = vmInstructor.Instructor.Email;

            var creationResult = await userManager.CreateAsync(user, vmInstructor.Instructor.Password);

            if (creationResult.Succeeded)
            {
                var instructor = sisContext.Instructor.FirstOrDefault(i => i.Id == vmInstructor.Instructor.Id);
                if (instructor != null)
                {
                    instructor.UserId = user.Id;
                    sisContext.SaveChanges();
                }

                var roleResult = await userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    await userManager.DeleteAsync(user);
                }
            }

            return creationResult;
        }

        public async Task<SignInResult> SignIn(SignInModel userData)
        {
            var result = await signInManager.PasswordSignInAsync(userData.Email, userData.Password, false, false);

            return result;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var user = httpContextAccessor.HttpContext?.User;
            return user != null ? await userManager.GetUserAsync(user) : null;
        }

        public async Task<UserProfileModel> GetCurrentUserProfileAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return null;

            var instructor = await sisContext.Instructor.Include("Department").FirstOrDefaultAsync(i => i.UserId == user.Id);

            if (instructor != null)
            {
                return new UserProfileModel
                {
                    Id = instructor.Id,
                    FirstName = instructor.FirstName,
                    LastName = instructor.LastName,
                    Email = instructor.Email,
                    DOB = instructor.DOB,
                    DepartmentId = instructor.DepartmentId,
                    DepartmentName = instructor.Department.Name,
                    FileName = instructor.FileName
                };
            }

            var student = await sisContext.Student.Include("Department").FirstOrDefaultAsync(s => s.UserId == user.Id);

            if (student != null)
            {
                return new UserProfileModel
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    DOB = student.DOB,
                    DepartmentId = student.DepartmentId,
                    DepartmentName = student.Department.Name,
                    FileName = student.FileName
                };
            }

            return null;
        }

        public async Task<string> GetCurrentUserRoleAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                return roles.FirstOrDefault();
            }
            return null;
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            var user = await GetCurrentUserAsync();

            var result = await userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword, changePasswordModel.NewPassword);

            return result;
        }

        public async Task DeleteAccount(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
        }
    }
}
