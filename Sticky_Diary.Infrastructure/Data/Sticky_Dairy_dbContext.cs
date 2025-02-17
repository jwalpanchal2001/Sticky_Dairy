using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sticky_Dairy.Domain.Models.Entities;

namespace Sticky_Dairy.Infrastructure.Data
{
    public class Sticky_Dairy_dbContext : DbContext
    {
        public Sticky_Dairy_dbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        
    }
}
