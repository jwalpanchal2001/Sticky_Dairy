using Sticky_Dairy.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky_Dairy.Application.Interfaces
{
    public interface IStickyNoteRepository : IRepository<Note>
    {
        Task<IEnumerable<Note>> GetNotesByUserEmailAsync(string email);
        Task<List<Note>> GetNotesById(Guid id);
        Task<Note> GetNoteById(Guid id);
        Task<Guid> CreateNotesAsync(Note note);


    }
}
