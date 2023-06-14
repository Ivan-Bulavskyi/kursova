using System.ComponentModel.DataAnnotations;

namespace AgroOrganic.Models
{
    public class Subscribe
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}