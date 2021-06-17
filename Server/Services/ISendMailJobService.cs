using System.Threading.Tasks;
using StoryForce.Shared.ViewModels;

namespace StoryForce.Server.Services
{
    public interface ISendMailJobService
    {
        Task SendEmailAsync(SendMailRequest sendMailRequest);
    }
    
}
