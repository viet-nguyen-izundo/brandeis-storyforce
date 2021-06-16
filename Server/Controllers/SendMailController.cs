using BackgroundEmailSenderSample.HostedServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController : Controller
    {
        private readonly SendMailService _sendMailService;
      
        public SendMailController(SendMailService sendMailService)
        {            
            _sendMailService = sendMailService ;
        }

        [HttpPost("SendMail")]
        public async Task<ActionResult> SendMail(string toEmail, string subject, string content)
        {
            await _sendMailService.SendEmailAsync(toEmail, subject, content);
            return Ok();
        }
    }
}
