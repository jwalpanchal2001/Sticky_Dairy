using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sticky_Dairy.Application.Interface;
using Sticky_Dairy.Application.Interfaces;
using Sticky_Dairy.Domain.Models.Entities;
using Sticky_Dairy.Infrastructure.Repositories;
using Sticky_Dairy.Infrastructure.Repository.Notes;
using StickyDiary.Application.Interfaces;

namespace Sticky_Dairy.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IReminderRepository _reminderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public NoteService(INoteRepository noteRepository, IAttachmentRepository attachmentRepository,
                           IReminderRepository reminderRepository, IHttpContextAccessor httpContextAccessor , IUserRepository userRepository)
        {
            _noteRepository = noteRepository;
            _attachmentRepository = attachmentRepository;
            _reminderRepository = reminderRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task CreateOrUpdateNoteAsync(Note note, List<IFormFile> attachments, DateTime? reminderAt, Guid? noteId)
        {
            var currentUserEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            if (currentUserEmail == null) throw new UnauthorizedAccessException("User not found.");

            //var userId = GetUserIdFromEmail(currentUserEmail);
            User user = await _userRepository.GetUserByEmailAsync(currentUserEmail);

            if (noteId == null || noteId == Guid.Empty)
            {
                note.UserId = user.UserId;
                note.CreatedAt = DateTime.UtcNow;
                note.UpdatedAt = DateTime.UtcNow;
                await _noteRepository.AddNoteAsync(note);
            }
            else
            {
                var existingNote = await _noteRepository.GetNoteByIdAsync(noteId.Value);
                if (existingNote == null) throw new KeyNotFoundException("Note not found.");

                existingNote.Title = note.Title;
                existingNote.content = note.content;
                existingNote.UpdatedAt = DateTime.UtcNow;
                await _noteRepository.UpdateNoteAsync(existingNote);
            }

            if (attachments != null && attachments.Count > 0)
            {
                foreach (var file in attachments)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var attachment = new Attachment
                    {
                        FileName = fileName,
                        FilePath = $"/uploads/{fileName}",
                        NoteID = note.NoteId,
                        FileType = file.ContentType,
                    };
                    await _attachmentRepository.AddAttachmentAsync(attachment);
                }
            }

            if (reminderAt.HasValue)
            {
                var reminder = new Reminder
                {
                    NoteID = note.NoteId,
                    ReminderAt = reminderAt.Value
                };
                await _reminderRepository.AddOrUpdateReminderAsync(reminder);
            }
        }

    
    }
}
