using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sticky_Dairy.Domain.Models.Entities
{
    public class Note
    {
        [Key]
        public Guid NoteId { get; set; }
        [Required]
        public Guid UserId { get; set; } // foreign key

        [MaxLength(255)]
        public string Title { get; set; }

        public string content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }= DateTime.UtcNow;

        //navigation property
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    }
}
