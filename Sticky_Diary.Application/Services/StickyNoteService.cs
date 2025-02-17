using Microsoft.EntityFrameworkCore;
using Sticky_Dairy.Application.Interfaces;
using Sticky_Dairy.Domain.Models.Entities;
using Sticky_Dairy.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky_Dairy.Application.Services
{
    public class StickyNoteService : IStickyNoteService
    {
        private readonly IStickyNoteRepository _stickyNoteRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<Reminder> _reminderRepository;
        private readonly IRepository<User> _userRepository;

        public StickyNoteService(
            IStickyNoteRepository stickyNoteRepository,
            IRepository<Attachment> attachmentRepository,
            IRepository<Reminder> reminderRepository,
            IRepository<User> userRepository)
        {
            _stickyNoteRepository = stickyNoteRepository;
            _attachmentRepository = attachmentRepository;
            _reminderRepository = reminderRepository;
            _userRepository = userRepository;
        }



        public async Task<IEnumerable<Note>> GetNotesByUserEmailAsync(string email)
        {
            return await _stickyNoteRepository.GetNotesByUserEmailAsync(email);
        }

        public async Task<Note> GetByIdAsync(Guid id)
        {
            return await _stickyNoteRepository.GetNoteById(id);
        }


        public async Task DeleteNoteAsync(Guid id)
        {
            await _stickyNoteRepository.DeleteAsync(id);
        }

        public async Task DeleteAttachmentAsync(Guid attachmentId)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(attachmentId);
            if (attachment != null)
            {
                File.Delete(Path.Combine("wwwroot", attachment.FilePath.TrimStart('/')));
                await _attachmentRepository.DeleteAsync(attachmentId);
            }
        }

        //public async Task<List<Note>> GetNotesByUserId(Guid id)
        //{
        //    return await _stickyNoteRepository.GetNotesById(id);
        //}


        //public async Task<Guid> CreateNoteAsync(Note note)
        //{
        //    return await _stickyNoteRepository.CreateNotesAsync(note);
        //}


    }
}
