using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sticky_Dairy.Domain.Models.Entities;

namespace Sticky_Dairy.Infrastructure.Repository.Notes
{
    public interface INoteRepository
    {
        Task<Note> GetNoteByIdAsync(Guid noteId);
        Task<List<Note>> GetUserNotesAsync(Guid userId);
        Task AddNoteAsync(Note note);
        Task UpdateNoteAsync(Note note);
    }
}
