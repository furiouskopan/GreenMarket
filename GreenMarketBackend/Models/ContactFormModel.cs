using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models
{
    public class ContactFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }

        public bool Subscribe { get; set; }
    }
}
