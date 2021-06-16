using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryForce.Server.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }

    public class SenGridMailService : IMailService
    {
        private IConfiguration _configuration;

        public SenGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendEmailAsync"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("viet.nguyen@izundo.com", "Test Send Mail");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var respon = await client.SendEmailAsync(msg);
        }
    }
}
