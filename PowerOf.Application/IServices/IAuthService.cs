using Microsoft.AspNetCore.Identity;
using PowerOf.Application.DTO_s;
using PowerOf.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Application.IServices
{
    public interface IAuthService
    {
        Task<User> FindByEmailAsync(string email);
        Task<IdentityResult> RegisterAsync(RegisterDTO registerDTO);
        Task<object> LoginAsync(LoginDTO loginDTO);
        Task<string> GeneratePasswordResetTokenAsync(ForgetPasswordDTO forgetPasswordDTO);
        Task<IdentityResult> UpdatePasswordAsync(UpdatePasswordDTO updatePasswordDTO);
        Task SendResetPasswordEmailAsync(string email);
        Task SignOutAsync();

        Task<bool> AssignRoleAsync(AssignRoleDTO assignRoleDto);
        Task<object> LoginWithGoogleAsync(GoogleLoginDTO googleLoginDTO);
    }
}
