using Microsoft.AspNetCore.Identity;
using PowerOf.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Core.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public int? VerificationCode { get; set; }
        public Roles Roles {  get; set; }

    }
}
