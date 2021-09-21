using System.ComponentModel.DataAnnotations;

namespace Web4Spain.Models
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(255)]
        public string Message { get; set; }
    }
}
