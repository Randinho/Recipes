using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Recipes.Services
{
    public class Mailer : IMailer
    {
        private readonly SmtpSettings smtpSettings;
        private readonly IWebHostEnvironment env;

        public Mailer(IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env)
        {
            this.smtpSettings = smtpSettings.Value;
            this.env = env;
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    if (env.IsDevelopment())
                    {
                        await client.ConnectAsync(smtpSettings.Server, smtpSettings.Port, true);
                    }
                    else
                    {
                        await client.ConnectAsync(smtpSettings.Server);
                    }

                    await client.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
