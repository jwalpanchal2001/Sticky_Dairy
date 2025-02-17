using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sticky_Dairy.Domain.Models.Entities
{
    public class Attachment
    {
        [Key]
        public Guid AttachmentID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid NoteID { get; set; } // Foreign Key

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; }

        [Required]
        [MaxLength(50)]
        public string FileType { get; set; } // e.g., "image/png", "application/pdf"

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        [ForeignKey("NoteID")]
        public Note Note { get; set; }
    }
}
