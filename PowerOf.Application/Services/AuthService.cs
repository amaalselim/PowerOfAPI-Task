using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PowerOf.Application.DTO_s;
using PowerOf.Application.IServices;
using PowerOf.Core.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Application.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _usermanager;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _manager;
        private readonly IEmailService _emailservice;

        public AuthService(UserManager<User> user, IConfiguration config,
            RoleManager<IdentityRole> roleManager, IMapper mapper, SignInManager<User> manager
            )
        {
            _usermanager = user;
            _config = config;
            _roleManager = roleManager;
            _mapper = mapper;
            _manager = manager;

            string smtpServer = _config["SMTP:Server"];
            int smtpPort = int.Parse(_config["SMTP:Port"]);
            string smtpUser = _config["SMTP:User"];
            string smtpPass = _config["SMTP:Password"];
            _emailservice = new EmailService(smtpServer, smtpPort, smtpUser, smtpPass);
        }

        public async Task<bool> AssignRoleAsync(AssignRoleDTO assignRoleDto)
        {
            var user = await _usermanager.FindByEmailAsync(assignRoleDto.Email);
            if (user == null)
            {
                return false;
            }
            if (!await _roleManager.RoleExistsAsync(assignRoleDto.Role.ToString()))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(assignRoleDto.Role.ToString()));
            }
            var result = await _usermanager.AddToRoleAsync(user, assignRoleDto.Role.ToString());
            return result.Succeeded;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _usermanager.FindByEmailAsync(email);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ForgetPasswordDTO forgetPasswordDTO)
        {
            var user = await _usermanager.FindByEmailAsync(forgetPasswordDTO.Email);
            if (user == null)
            {
                return null;
            }
            return await _usermanager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<object> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _usermanager.FindByEmailAsync(loginDTO.Email);
            if (user != null)
            {

                var result = await _manager.PasswordSignInAsync(user.Email, loginDTO.Password, loginDTO.RememberMe, false);
                if (result.Succeeded)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, loginDTO.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var roles = await _usermanager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
                    SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var tokenExpiration = loginDTO.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddHours(20);

                    JwtSecurityToken token = new JwtSecurityToken(
                        issuer: _config["JWT:Issuer"],
                        audience: _config["JWT:Audience"],
                        expires: tokenExpiration,
                        claims: claims,
                        signingCredentials: signingCredentials
                    );

                    return new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expire = tokenExpiration
                    };
                }
            }
            return new { Error = "Invalid email or password." };
        }

        public async Task<SignInResult> PasswordSignInAsync(LoginDTO loginDto)
        {
            return await _manager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RememberMe, false);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = _mapper.Map<User>(registerDTO);
            return await _usermanager.CreateAsync(user, registerDTO.Password);
        }

        public async Task<object> LoginWithGoogleAsync(GoogleLoginDTO googleLoginDTO)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(googleLoginDTO.IdToken);
            if (payload == null)
            {
                throw new Exception("Invalid Google ID Token");
            }
            var user = await _usermanager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    FullName = payload.Name,
                    UserName = payload.Email,
                    Email = payload.Email

                };
                await _usermanager.CreateAsync(user);
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddDays(30);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        public async Task SendResetPasswordEmailAsync(string email)
        {
            var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }

            Random random = new Random();
            int verificationCode = random.Next(1000, 9999);


            user.VerificationCode = verificationCode;
            await _usermanager.UpdateAsync(user);


            await _emailservice.SendEmailAsync(email, user.FullName, "Reset Your Password",
            $"Your verification code is:<br/><code style='font-size: 18px; color: #3498db;'>{verificationCode}</code>");

        }

        public async Task SignOutAsync()
        {
            await _manager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdatePasswordAsync(UpdatePasswordDTO updatePasswordDTO)
        {
            var user = await _usermanager.FindByEmailAsync(updatePasswordDTO.Email);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var token = await _usermanager.GeneratePasswordResetTokenAsync(user);
            var result = await _usermanager.ResetPasswordAsync(user, token, updatePasswordDTO.NewPassword);

            if (result.Succeeded)
            {
                user.VerificationCode = null;
                await _usermanager.UpdateAsync(user);
            }
            return result;
        }
    }
}
