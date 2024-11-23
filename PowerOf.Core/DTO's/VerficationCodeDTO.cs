﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Application.DTO_s
{
    public class VerficationCodeDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string email { get; set; }
        [Required(ErrorMessage = "VerficationCode is required.")]
        public string code { get; set; }
    }
}
