using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sticky_Dairy.Domain.Models.Entities;

namespace Sticky_Dairy.Application.Interface
{
    public interface INoteService
    {
        Task CreateOrUpdateNoteAsync(Note note, List<IFormFile> attachments, DateTime? reminderAt, Guid? noteId);
    }

}
