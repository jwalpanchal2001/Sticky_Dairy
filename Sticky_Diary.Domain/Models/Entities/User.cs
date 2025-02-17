using System.ComponentModel.DataAnnotations;

namespace Sticky_Dairy.Domain.Models.Entities
{
    public class User 
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required][EmailAddress]
        public string EmailAddress { get; set; }    
        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();


    }
}
