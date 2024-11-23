using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Application.DTO_s
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "password is required.")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public bool RememberMe { get; set; }
    }
}
