using Microsoft.EntityFrameworkCore;
using Sticky_Dairy.Application.Interfaces;
using Sticky_Dairy.Domain.Models.Entities;
using Sticky_Dairy.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky_Dairy.Infrastructure.Repositories
{
    public class StickyNoteRepository : Repository<Note>, IStickyNoteRepository
    {
        public StickyNoteRepository(Sticky_Dairy_dbContext context) : base(context) { }

        public async Task<IEnumerable<Note>> GetNotesByUserEmailAsync(string email)
        {
            return await _context.Notes
                .Include(n => n.User)
                .Include(n => n.Attachments)
                .Include(n => n.Reminders)
                .Where(n => n.User != null && n.User.EmailAddress == email)
                .ToListAsync();
        }

       public async Task<List<Note>> GetNotesById(Guid id)
        {
            return await _context.Notes.Include(n => n.User).Include(n => n.Attachments).Include(n => n.Reminders).
                Where(n=>n.User != null && n.UserId == id).ToListAsync();

        }

        public async Task<Note> GetNoteById(Guid id)
        {
            return await _context.Notes.Include(n => n.User).Include(n => n.Attachments).Include(m => m.Reminders).Where(m => m.NoteId == id).FirstOrDefaultAsync();
        }


        public async Task<Guid> CreateNotesAsync(Note note)
        {
            if (note == null)
            {
                return Guid.Empty; 
            }

            _context.Notes.Add(note);
            await _context.SaveChangesAsync(); 

            return note.NoteId; 
        }
    }
}
