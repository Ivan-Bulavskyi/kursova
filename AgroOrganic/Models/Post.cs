using System.ComponentModel.DataAnnotations;

namespace AgroOrganic.Models
{
    public class Post
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }

        public int RoleId { get; set; }
    }
}