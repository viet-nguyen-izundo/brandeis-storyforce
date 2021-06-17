using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using StoryForce.Server.Services;
using StoryForce.Shared.ViewModels;

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController : Controller
    {
        private readonly SendMailJobService _sendMailJobService;
      
        public SendMailController(SendMailJobService sendMailJobService)
        {            
            _sendMailJobService = sendMailJobService ;
        }

        [HttpPost]
        public async Task<ActionResult> SendMail(SendMailRequest sendMailRequest)
        {
            await _sendMailJobService.SendEmailAsync(sendMailRequest);
            return Ok();
        }
    }
}
