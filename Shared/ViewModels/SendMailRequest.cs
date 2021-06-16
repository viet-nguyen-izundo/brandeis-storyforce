using System.ComponentModel.DataAnnotations;

namespace StoryForce.Shared.ViewModels
{
    public class SendMailRequest
    {
        [Required]
        public string To { get; set; }

        [Required]
        public string Subject { get; set; }

        public string Content { get; set; }
    }
}