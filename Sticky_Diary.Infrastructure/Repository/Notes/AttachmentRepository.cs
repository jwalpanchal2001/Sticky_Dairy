using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sticky_Dairy.Domain.Models.Entities;
using Sticky_Dairy.Infrastructure.Data;
using StickyDiary.Application.Interfaces;

namespace StickyDiary.Infrastructure.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly Sticky_Dairy_dbContext _context;

        public AttachmentRepository(Sticky_Dairy_dbContext context)
        {
            _context = context;
        }

        public async Task AddAttachmentAsync(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
        }
    }
}
