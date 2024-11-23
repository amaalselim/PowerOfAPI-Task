﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerOf.Application.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string userName, string Subject, string message);
    }
}
