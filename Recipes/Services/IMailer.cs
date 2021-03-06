﻿using System.Threading.Tasks;

namespace Recipes.Services
{
    public interface IMailer
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}
