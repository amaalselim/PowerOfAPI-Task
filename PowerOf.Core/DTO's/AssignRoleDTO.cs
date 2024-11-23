﻿using PowerOf.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Application.DTO_s
{
    public class AssignRoleDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public Roles Role { get; set; }
    }
}
