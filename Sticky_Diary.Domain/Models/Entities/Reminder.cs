using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sticky_Dairy.Domain.Models.Entities
{
    public class Reminder
    {
        [Key]
        public Guid ReminderID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid NoteID { get; set; } // Foreign Key

        [Required]
        public DateTime ReminderAt { get; set; }
        //[Required]
        //public DateTime ReminderBefore { get; set; }

        //[Required]
        //public bool PopReminderOnorOff { get; set; };

        //[Required]
        //public DateTime 

        public bool IsCompleted { get; set; } = false;

        // Navigation Propertys
        [ForeignKey("NoteID")]
        public Note Note { get; set; }
    }
}
