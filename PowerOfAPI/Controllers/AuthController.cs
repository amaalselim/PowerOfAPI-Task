using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerOf.Application.DTO_s;
using PowerOf.Application.IServices;
using PowerOf.Application.Services;

namespace PowerOfAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var result = await _authService.RegisterAsync(registerDTO);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User Registered Successfully." });
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _authService.LoginAsync(loginDTO);
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest("Invalid Login Attempt.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            var token = await _authService.GeneratePasswordResetTokenAsync(forgetPasswordDTO);
            if (token == null)
            {
                return BadRequest("Failed to generate reset token.");
            }

            try
            {

                await _authService.SendResetPasswordEmailAsync(forgetPasswordDTO.Email);
                return Ok(new { message = "Reset password email sent successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }

        }
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.UpdatePasswordAsync(updatePasswordDTO);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password updated successfully." });
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }
        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerficationCodeDTO verficationCodeDTO)
        {
            var user = await _authService.FindByEmailAsync(verficationCodeDTO.email);
            if (user == null)
            {
                return BadRequest(new { error = "User not found." });
            }
            if (user.VerificationCode.ToString() != verficationCodeDTO.code)
            {
                return BadRequest(new { error = "Invalid verification code." });
            }
            return Ok(new { message = "Verification successful." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync();
            return Ok(new { Message = "User logged out successfully." });
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleLoginDTO googleLoginDTO)
        {
            var result = await _authService.LoginWithGoogleAsync(googleLoginDTO);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(new { message = "Invalid Google login." });
        }
        [HttpPost("assign-role")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO assignRoleDto)
        {
            if (assignRoleDto == null) return BadRequest();
            var result = await _authService.AssignRoleAsync(assignRoleDto);
            if (!result)
            {
                return NotFound("User Not Found or Could not assign role.");

            }
            return Ok("Role assigned successfully.");
        }
    }
}
