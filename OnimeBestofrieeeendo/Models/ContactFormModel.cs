using System.ComponentModel.DataAnnotations;

namespace OnimeBestofrieeeendo.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Haha, are you ashamed to enter your name? Come on, show me your stand!")]
        public string Name { get; set; } = "";
        
        [Required(ErrorMessage = "Email! Give me your email!!!")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";
        
        [Required(ErrorMessage = "Subject is required, bro!")]
        public string Subject { get; set; } = "";
        
        [Required(ErrorMessage = "Is that all you can do? Just say what you really want — write your own text already!")]
        public string Message { get; set; } = "";
    }
}