using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sticky_Dairy.Domain.Models.Entities;
using Sticky_Dairy.Infrastructure.Data;

namespace Sticky_Dairy.Infrastructure.Repository.Notes
{
    public class NoteRepository : INoteRepository
    {
        private readonly Sticky_Dairy_dbContext _context;

        public NoteRepository(Sticky_Dairy_dbContext context)
        {
            _context = context;
        }

        public async Task<Note> GetNoteByIdAsync(Guid noteId)
        {
            return await _context.Notes.Include(n => n.Attachments).Include(n => n.Reminders)
                .FirstOrDefaultAsync(n => n.NoteId == noteId);
        }

        public async Task<List<Note>> GetUserNotesAsync(Guid userId)
        {
            return await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
        }

        public async Task AddNoteAsync(Note note)
        {
            // Ensure the user exists before assigning
            var user = await _context.Users.FindAsync(note.UserId);

            if (user == null)
            {
                throw new Exception("User not found. Cannot create a note.");
            }

            note.User = user;

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNoteAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }
    }
}
