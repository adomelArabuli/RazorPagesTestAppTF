﻿using RazorPagesTestAppTF.Data.DbModels;
using RazorPagesTestAppTF.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace RazorPagesTestAppTF.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var options = _configuration.GetSection("Credentials").Get<EmailSenderOptions>();

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(options.Email, options.Password)
            };

            var mailMessage = new MailMessage(from: options.Email,
                to: email,
                subject,
                message);

            mailMessage.IsBodyHtml = true;

            return client.SendMailAsync(mailMessage);
        }
    }
}
