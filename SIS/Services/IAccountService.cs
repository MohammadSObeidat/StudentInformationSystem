using Microsoft.AspNetCore.Identity;
using SIS.Data;
using SIS.Models;

namespace SIS.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateStudentAccount(VmStudent vmStudent, string role);
        Task<IdentityResult> CreateInstructorAccount(VmInstructor vmInstructor, string role);
        Task<SignInResult> SignIn(SignInModel userData);
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<UserProfileModel> GetCurrentUserProfileAsync();
        Task<string> GetCurrentUserRoleAsync();
        Task Logout();
        Task<IdentityResult> ChangePassword(ChangePasswordModel changePasswordModel);
    }
}