using System.ComponentModel.DataAnnotations;

namespace AgroOrganic.Models
{
    public class Feedback
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Text { get; set; }
    }
}