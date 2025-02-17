using Microsoft.AspNetCore.Http;
using Sticky_Dairy.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky_Dairy.Application.Interfaces
{
    public interface IStickyNoteService
    {
        Task<IEnumerable<Note>> GetNotesByUserEmailAsync(string email);
        //Task<List<Note>> GetNotesByUserId(Guid id);
        Task<Note> GetByIdAsync(Guid id);
        Task DeleteNoteAsync(Guid id);
        Task DeleteAttachmentAsync(Guid attachmentId);
        //Task<Guid> CreateNoteAsync(Note note);


    }
}
