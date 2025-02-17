using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sticky_Dairy.Domain.Models.Entities;

namespace StickyDiary.Application.Interfaces
{
    public interface IAttachmentRepository
    {
        Task AddAttachmentAsync(Attachment attachment);
    }
}
