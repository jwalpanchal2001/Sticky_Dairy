
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sticky_Dairy.Application.Interface;
using Sticky_Dairy.Application.Interfaces;
using Sticky_Dairy.Application.Services;
using Sticky_Dairy.Domain.Models.Entities;
using StickyDiary.Application.Interfaces;
using System.Security.Claims;

[Authorize]
public class NotesController : Controller
{
    private readonly IStickyNoteService _stickyNoteService;
    private readonly IUserService _userService;
    private readonly INoteService _noteService;

    public NotesController(IStickyNoteService stickyNoteService , 
        IUserService userService,INoteService noteService)
    {
        _stickyNoteService = stickyNoteService;
        _userService = userService;
        _noteService = noteService;
    }

    public async Task<IActionResult> Index()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var notes = await _stickyNoteService.GetNotesByUserEmailAsync(email);
        return View(notes);
    }
    public async Task<IActionResult> Create(Guid? noteId)
    {
        if (noteId == null || noteId == Guid.Empty)
        {
            Console.WriteLine("Opening Create Modal (New Note)");
            return PartialView("_NoteForm", new Note());
        }
        else
        {
            Console.WriteLine($"Opening Edit Modal for Note ID: {noteId}");
            if (!noteId.HasValue)
            {

                return BadRequest("Invlide Id");
            }
            var note = await _stickyNoteService.GetByIdAsync(noteId.Value);

            if (note == null)
            {
                return NotFound();
            }

            return PartialView("_NoteForm", note);
        }

    }

    [HttpPost]
    public async Task<IActionResult> Create(Note model, List<IFormFile> attachments, DateTime? reminderAt, Guid? noteId)
    {
        await _noteService.CreateOrUpdateNoteAsync(model, attachments, reminderAt, noteId);
        return RedirectToAction("Index");
    }






    public async Task<IActionResult> Edit(Guid id)
    {
        var note = await _stickyNoteService.GetByIdAsync(id);
        return note == null ? NotFound() : PartialView("_NoteForm", note);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await _stickyNoteService.DeleteNoteAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAttachment(Guid id)
    {
        await _stickyNoteService.DeleteAttachmentAsync(id);
        return Json(new { success = true });
    }
}



//using System.Runtime.InteropServices;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualBasic.FileIO;
//using Sticky_Dairy.Infrastructure.Data;
//using Sticky_Dairy.Api.Models.Entities;

//namespace Sticky_Dairy.Api.Controllers
//{
//    [Authorize]
//    public class NotesController : Controller
//    {
//        private readonly Sticky_Dairy_dbContext _context;

//        public NotesController(Sticky_Dairy_dbContext sticky_Dairy_DbContext)
//        {
//            this._context = sticky_Dairy_DbContext;
//        }
//        public IActionResult Index()
//        {
//            var userId = User.FindFirstValue(ClaimTypes.Email);
//            var notes = _context.Notes.Where(n => n.User.EmailAddress == userId).Include(m => m.Attachments).Include(m => m.Reminders).ToList();
//            return View(notes);
//        }




//        public IActionResult Create(Guid? noteId)
//        {
//            if (noteId == null || noteId == Guid.Empty)
//            {
//                Console.WriteLine("Opening Create Modal (New Note)");
//                return PartialView("_NoteForm", new Note());
//            }
//            else
//            {
//                Console.WriteLine($"Opening Edit Modal for Note ID: {noteId}");
//                var note = _context.Notes
//                    .Include(n => n.Attachments)
//                    .Include(n => n.Reminders)
//                    .FirstOrDefault(n => n.NoteId == noteId);

//                if (note == null)
//                {
//                    return NotFound();
//                }

//                return PartialView("_NoteForm", note);
//            }
//        }






//        [HttpPost]
//        public IActionResult Create(Note model, List<IFormFile> attachments, DateTime? ReminderAt, Guid? NoteId)
//        {
//            // If 'id' is null, create a new note
//            if (model.NoteId == null || model.NoteId == Guid.Empty)
//            {
//                if (ModelState.IsValid)
//                {
//                    var currentUser = _context.Users.FirstOrDefault(u => u.EmailAddress == User.FindFirstValue(ClaimTypes.Email));
//                    if (currentUser != null)
//                    {
//                        model.UserId = currentUser.UserId;
//                    }

//                    model.CreatedAt = DateTime.UtcNow;
//                    model.UpdatedAt = DateTime.UtcNow;

//                    // Add the new note to the database
//                    _context.Notes.Add(model);
//                    _context.SaveChanges(); // Save changes first to get the NoteId

//                    // Handle attachments
//                    if (attachments != null && attachments.Count > 0)
//                    {
//                        foreach (var file in attachments)
//                        {
//                            if (file.Length > 0)
//                            {
//                                var fileName = Path.GetFileName(file.FileName);
//                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

//                                var fileDirectory = Path.GetDirectoryName(filePath);
//                                if (!Directory.Exists(fileDirectory))
//                                {
//                                    Directory.CreateDirectory(fileDirectory);
//                                }

//                                // Save the file to the server
//                                using (var stream = new FileStream(filePath, FileMode.Create))
//                                {
//                                    file.CopyTo(stream);
//                                }

//                                // Add attachment to the database
//                                var attachment = new Attachment
//                                {
//                                    FileName = fileName,
//                                    FilePath = $"/uploads/{fileName}",
//                                    NoteID = model.NoteId,
//                                    FileType = file.ContentType,
//                                };
//                                _context.Attachments.Add(attachment);
//                            }
//                        }
//                        _context.SaveChanges();
//                    }

//                    // Handle reminder if it's set
//                    if (ReminderAt.HasValue)
//                    {
//                        var reminder = new Reminder
//                        {
//                            NoteID = model.NoteId,
//                            ReminderAt = ReminderAt.Value
//                        };
//                        _context.Reminders.Add(reminder);
//                        _context.SaveChanges();
//                    }

//                    return RedirectToAction("Index");
//                }

//                return View(model);
//            }
//            else
//            {
//                // Edit an existing note
//                var existingNote = _context.Notes.Include(m => m.Attachments).Include(m => m.Reminders).FirstOrDefault(m => m.NoteId == NoteId);

//                if (existingNote != null)
//                {
//                    existingNote.Title = model.Title;
//                    existingNote.content = model.content;
//                    existingNote.UpdatedAt = DateTime.UtcNow;

//                    // Update attachments (if new ones are uploaded)
//                    if (attachments != null && attachments.Count > 0)
//                    {
//                        foreach (var file in attachments)
//                        {
//                            if (file.Length > 0)
//                            {
//                                var fileName = Path.GetFileName(file.FileName);
//                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

//                                var fileDirectory = Path.GetDirectoryName(filePath);
//                                if (!Directory.Exists(fileDirectory))
//                                {
//                                    Directory.CreateDirectory(fileDirectory);
//                                }

//                                using (var stream = new FileStream(filePath, FileMode.Create))
//                                {
//                                    file.CopyTo(stream);
//                                }

//                                var attachment = new Attachment
//                                {
//                                    FileName = fileName,
//                                    FilePath = $"/uploads/{fileName}",
//                                    NoteID = existingNote.NoteId,
//                                    FileType = file.ContentType,
//                                };
//                                _context.Attachments.Add(attachment);
//                            }
//                        }
//                    }

//                    // Update reminder if it's set
//                    if (ReminderAt.HasValue)
//                    {
//                        var existingReminder = _context.Reminders.FirstOrDefault(r => r.NoteID == existingNote.NoteId);
//                        if (existingReminder != null)
//                        {
//                            existingReminder.ReminderAt = ReminderAt.Value;
//                            _context.Reminders.Update(existingReminder);
//                        }
//                        else
//                        {
//                            // If no existing reminder, create a new one
//                            var reminder = new Reminder
//                            {
//                                NoteID = existingNote.NoteId,
//                                ReminderAt = ReminderAt.Value
//                            };
//                            _context.Reminders.Add(reminder);
//                        }
//                    }

//                    _context.SaveChanges();

//                    return RedirectToAction("Index");
//                }

//                return NotFound(); // Handle case if the note is not found.
//            }
//        }



//        public async Task<IActionResult> Edit(Guid id)
//        {
//            var note = await _context.Notes
//                .Include(n => n.Reminders)
//                .Include(n => n.Attachments)
//                .FirstOrDefaultAsync(n => n.NoteId == id);

//            if (note == null)
//            {
//                return NotFound();
//            }

//            return PartialView("_NoteForm", note);
//        }

//        public IActionResult Delete(Guid id)
//        {
//            // Find the note by its ID
//            var note = _context.Notes.Include(n => n.Attachments).Include(n => n.Reminders).FirstOrDefault(n => n.NoteId == id);

//            if (note == null)
//            {
//                return NotFound();  // Return 404 if the note is not found
//            }

//            // Delete attachments from the server
//            foreach (var attachment in note.Attachments)
//            {
//                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachment.FilePath.TrimStart('/'));
//                if (System.IO.File.Exists(filePath))
//                {
//                    System.IO.File.Delete(filePath); // Delete the file from the server
//                }
//            }


//            // Remove the attachments from the database
//            _context.Attachments.RemoveRange(note.Attachments);
//            _context.Reminders.RemoveRange(note.Reminders);


//            _context.Notes.Remove(note);

//            _context.SaveChanges();

//            return RedirectToAction("Index");  // Redirect to the list of notes or another page
//        }



//        [HttpPost]
//        public IActionResult DeleteAttachment(Guid id)
//        {
//            var attachment = _context.Attachments.FirstOrDefault(a => a.AttachmentID == id);
//            if (attachment != null)
//            {
//                // Delete the file from the server
//                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachment.FilePath.TrimStart('/'));
//                if (System.IO.File.Exists(filePath))
//                {
//                    System.IO.File.Delete(filePath);
//                }

//                // Remove from the database
//                _context.Attachments.Remove(attachment);
//                _context.SaveChanges();

//                return Json(new { success = true });
//            }
//            return Json(new { success = false, message = "Attachment not found." });
//        }




//    }
//}
